using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Services;
using Ewan.Core.Specifications;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using CoreClient = global::Ewan.Core.Models.Client;

namespace Ewan.Application.Features.Auth.Commands.LoginClient
{
    public class LoginClientCommandHandler : IRequestHandler<LoginClientCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<CoreClient> _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IFirebaseNotificationService _firebaseNotificationService;
        private readonly IConfiguration _configuration;

        public LoginClientCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher<CoreClient> passwordHasher,
            ITokenService tokenService,
            IFirebaseNotificationService firebaseNotificationService,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _firebaseNotificationService = firebaseNotificationService;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Handle(LoginClientCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var clientRepo = _unitOfWork.Repository<global::Ewan.Core.Models.Client>();

            var client = await clientRepo.GetEntityWithSpec(
                new ClientByEmailSpecification(request.Email.Trim()));

            if (client == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!client.IsActive)
                throw new InvalidOperationException("حسابك غير نشط تواصل مع الدعم");

            var verifyResult = _passwordHasher.VerifyHashedPassword(client, client.PasswordHash, request.Password);
            if (verifyResult == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!string.IsNullOrWhiteSpace(request.DeviceToken))
            {
                var deviceToken = request.DeviceToken.Trim();
                if (!string.Equals(client.DeviceToken, deviceToken, StringComparison.Ordinal))
                {
                    client.DeviceToken = deviceToken;
                    clientRepo.Update(client);
                    await _unitOfWork.SaveChangesAsync();
                }

                await _firebaseNotificationService.SubscribeTokenToAllClientsTopicAsync(deviceToken, cancellationToken);
            }

            var (accessToken, refreshToken) = await _tokenService.CreateTokenAsync(
                client,
                command.IpAddress,
                request.DeviceInfo,
                request.DeviceId,
                request.RememberMe);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserType = "Client",
                UserId = client.Id.ToString(),
                Email = client.Email,
                UserName = client.FullName,
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