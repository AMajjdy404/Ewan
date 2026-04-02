using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Services;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Ewan.Application.Features.Auth.Commands.AppRefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public RefreshTokenCommandHandler(
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            UserManager<AppUser> userManager,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                throw new UnauthorizedAccessException("Refresh token is required");

            var (newAccessToken, newRefreshToken) = await _tokenService.RefreshTokenAsync(
                request.RefreshToken,
                command.IpAddress,
                request.DeviceInfo,
                request.DeviceId);

            var refreshTokenRepo = _unitOfWork.Repository<RefreshToken>();
            var hashedNewRefreshToken = ComputeSha256Hash(newRefreshToken);

            var storedToken = await refreshTokenRepo.GetEntityWithSpec(
                new RefreshTokenByHashSpecification(hashedNewRefreshToken));

            if (storedToken == null)
                throw new SecurityTokenException("Failed to load refreshed token");

            string? email = null;
            string? userName = null;

            if (storedToken.UserType == "AppUser")
            {
                var user = await _userManager.FindByIdAsync(storedToken.OwnerId);
                if (user != null)
                {
                    email = user.Email;
                    userName = user.UserName;
                }
            }
            else if (storedToken.UserType == "Client")
            {
                var clientRepo = _unitOfWork.Repository<global::Ewan.Core.Models.Client>();

                if (int.TryParse(storedToken.OwnerId, out var clientId))
                {
                    var client = await clientRepo.GetByIdAsync(clientId);
                    if (client != null)
                    {
                        email = client.Email;
                        userName = client.FullName;
                    }
                }
            }

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                UserType = storedToken.UserType,
                UserId = storedToken.OwnerId,
                Email = email,
                UserName = userName,
                ExpiresAtUtc = GetAccessTokenExpirationUtc()
            };
        }

        private DateTime GetAccessTokenExpirationUtc()
        {
            if (!double.TryParse(_configuration["JWT:AccessTokenDurationInMinutes"], out var minutes))
                throw new InvalidOperationException("Invalid JWT duration.");

            return DateTime.UtcNow.AddMinutes(minutes);
        }

        private static string ComputeSha256Hash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input.Trim());
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}