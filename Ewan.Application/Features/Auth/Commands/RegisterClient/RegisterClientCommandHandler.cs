using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Services;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Ewan.Application.Features.Auth.Commands.RegisterClient
{

    namespace Ewan.Application.Features.Auth.Commands.RegisterClient
    {
        public class RegisterClientCommandHandler : IRequestHandler<RegisterClientCommand, AuthResponseDto>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IPasswordHasher<Client> _passwordHasher;
            private readonly ITokenService _tokenService;
            private readonly IConfiguration _configuration;

            public RegisterClientCommandHandler(
                IUnitOfWork unitOfWork,
                IPasswordHasher<Client> passwordHasher,
                ITokenService tokenService,
                IConfiguration configuration)
            {
                _unitOfWork = unitOfWork;
                _passwordHasher = passwordHasher;
                _tokenService = tokenService;
                _configuration = configuration;
            }

            public async Task<AuthResponseDto> Handle(RegisterClientCommand command, CancellationToken cancellationToken)
            {
                var request = command.Request;

                var clientRepo = _unitOfWork.Repository<Client>();

                var existingClient = await clientRepo.GetEntityWithSpec(
                    new ClientByEmailSpecification(request.Email.Trim()));

                if (existingClient != null)
                    throw new InvalidOperationException("Email is already registered.");

                var client = new Client
                {
                    FullName = request.FullName.Trim(),
                    Email = request.Email.Trim(),
                    PhoneNumber = request.PhoneNumber?.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                client.PasswordHash = _passwordHasher.HashPassword(client, request.Password);

                await clientRepo.AddAsync(client);
                await _unitOfWork.SaveChangesAsync();

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
}
