using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Ewan.Application.Features.Auth.Commands.LoginAppUser
{
    public class LoginAppUserCommandHandler : IRequestHandler<LoginAppUserCommand, AuthResponseDto>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public LoginAppUserCommandHandler(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Handle(LoginAppUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var user = await _userManager.FindByEmailAsync(request.Email.Trim());
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid credentials");

            var (accessToken, refreshToken) = await _tokenService.CreateTokenAsync(
                user,
                command.IpAddress,
                request.DeviceInfo,
                request.DeviceId,
                request.RememberMe);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserType = "AppUser",
                UserId = user.Id.ToString(),
                Email = user.Email,
                UserName = user.UserName,
                ExpiresAtUtc = GetAccessTokenExpirationUtc()
            };
        }

        private DateTime GetAccessTokenExpirationUtc()
        {
            if (!double.TryParse(_configuration["JWT:AccessTokenDurationInMinutes"], out var minutes))
                throw new InvalidOperationException("Invalid JWT duration.");

            return DateTime.UtcNow.AddMinutes(minutes);
        }
    }
}