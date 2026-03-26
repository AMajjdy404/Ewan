using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Mail;
using Ewan.Core.Services;
using Ewan.Infrastructure.ReposAndSpecs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Ewan.Application
{
    

    public class AuthService : IAuthService
    {
        private const string AppUserType = "AppUser";
        private const string ClientUserType = "Client";

        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordHasher<Client> _clientPasswordHasher;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public AuthService(
            UserManager<AppUser> userManager,
            IPasswordHasher<Client> clientPasswordHasher,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IMailService mailService)
        {
            _userManager = userManager;
            _clientPasswordHasher = clientPasswordHasher;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mailService = mailService;
        }

        public async Task<AuthResponseDto> LoginAppUserAsync(LoginRequestDto request, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid credentials");

            var (accessToken, refreshToken) = await _tokenService.CreateTokenAsync(
                user,
                ipAddress,
                request.DeviceInfo,
                request.DeviceId,
                request.RememberMe);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserType = AppUserType,
                UserId = user.Id.ToString(),
                Email = user.Email,
                UserName = user.UserName,
                ExpiresAtUtc = GetAccessTokenExpirationUtc()
            };
        }

        public async Task<AuthResponseDto> LoginClientAsync(ClientLoginRequestDto request, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var clientRepo = _unitOfWork.Repository<Client>();
            var client = await clientRepo.GetEntityWithSpec(new ClientByEmailSpecification(request.Email.Trim()));

            if (client == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var verifyResult = _clientPasswordHasher.VerifyHashedPassword(client, client.PasswordHash, request.Password);
            if (verifyResult == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid credentials");

            var (accessToken, refreshToken) = await _tokenService.CreateTokenAsync(
                client,
                ipAddress,
                request.DeviceInfo,
                request.DeviceId,
                request.RememberMe);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserType = ClientUserType,
                UserId = client.Id.ToString(),
                Email = client.Email,
                UserName = client.FullName,
                ExpiresAtUtc = GetAccessTokenExpirationUtc()
            };
        }

        public async Task<AuthResponseDto> RegisterClientAsync(RegisterClientDto request, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(request.FullName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                throw new BadHttpRequestException("Invalid registration data");
            }

            if (!string.Equals(request.Password, request.ConfirmPassword, StringComparison.Ordinal))
                throw new BadHttpRequestException("Passwords do not match");

            var clientRepo = _unitOfWork.Repository<Client>();

            var existingClient = await clientRepo.GetEntityWithSpec(
                new ClientByEmailSpecification(request.Email.Trim()));

            if (existingClient != null)
                throw new InvalidOperationException("Email is already registered");

            var client = new Client
            {
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim(),
                PhoneNumber = request.PhoneNumber?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            client.PasswordHash = _clientPasswordHasher.HashPassword(client, request.Password);

            await clientRepo.AddAsync(client);
            await _unitOfWork.SaveChangesAsync();

            var (accessToken, refreshToken) = await _tokenService.CreateTokenAsync(
                client,
                ipAddress,
                request.DeviceInfo,
                request.DeviceId,
                request.RememberMe);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserType = ClientUserType,
                UserId = client.Id.ToString(),
                Email = client.Email,
                UserName = client.FullName,
                ExpiresAtUtc = GetAccessTokenExpirationUtc()
            };
        }

        public async Task RequestClientPasswordResetAsync(ForgotClientPasswordDto request, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return;

            var clientRepo = _unitOfWork.Repository<Client>();
            var resetTokenRepo = _unitOfWork.Repository<ClientPasswordResetToken>();

            var client = await clientRepo.GetEntityWithSpec(
                new ClientByEmailSpecification(request.Email.Trim()));

            // Generic response behavior: do not reveal whether email exists
            if (client == null)
                return;

            var activeTokens = await resetTokenRepo.ListAsync(
                new ActiveClientPasswordResetTokensSpecification(client.Id));

            if (activeTokens.Count > 0)
            {
                resetTokenRepo.RemoveRange(activeTokens);
                await _unitOfWork.SaveChangesAsync();
            }

            var plainToken = GenerateSecureToken();
            var tokenHash = ComputeSha256Hash(plainToken);

            var resetToken = new ClientPasswordResetToken
            {
                ClientId = client.Id,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow,
                CreatedByIpAddress = ipAddress
            };

            await resetTokenRepo.AddAsync(resetToken);
            await _unitOfWork.SaveChangesAsync();

            var email = new Email
            {
                To = client.Email,
                Subject = "Reset your password",
                IsHtml = true,
                Body = $"""
                    <p>Hello {client.FullName},</p>
                    <p>We received a request to reset your password.</p>
                    <p>Your reset code is:</p>
                    <h2>{plainToken}</h2>
                    <p>This code expires in 30 minutes.</p>
                    <p>If you did not request this, you can ignore this email.</p>
                    """
            };

            await _mailService.SendEmailAsync(email);
        }

        public async Task ResetClientPasswordAsync(ResetClientPasswordDto request, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Code) ||
                string.IsNullOrWhiteSpace(request.NewPassword) ||
                string.IsNullOrWhiteSpace(request.ConfirmNewPassword))
            {
                throw new BadHttpRequestException("Invalid reset data");
            }

            if (!string.Equals(request.NewPassword, request.ConfirmNewPassword, StringComparison.Ordinal))
                throw new BadHttpRequestException("Passwords do not match");

            var clientRepo = _unitOfWork.Repository<Client>();
            var resetTokenRepo = _unitOfWork.Repository<ClientPasswordResetToken>();

            var client = await clientRepo.GetEntityWithSpec(
                new ClientByEmailSpecification(request.Email.Trim()));

            if (client == null)
                throw new UnauthorizedAccessException("Invalid reset request");

            var tokenHash = ComputeSha256Hash(request.Code.Trim());

            var resetToken = await resetTokenRepo.GetEntityWithSpec(
                new ClientPasswordResetTokenByHashSpecification(client.Id, tokenHash));

            if (resetToken == null)
                throw new SecurityTokenException("Invalid or expired reset code");

            client.PasswordHash = _clientPasswordHasher.HashPassword(client, request.NewPassword);
            clientRepo.Update(client);

            resetToken.IsUsed = true;
            resetToken.UsedAt = DateTime.UtcNow;
            resetTokenRepo.Update(resetToken);

            await _unitOfWork.SaveChangesAsync();

            await _tokenService.RevokeAllUserRefreshTokensAsync(
                ownerId: client.Id.ToString(),
                userType: ClientUserType,
                ipAddress: ipAddress,
                reason: "Password reset");
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                throw new UnauthorizedAccessException("Refresh token is required");

            var (newAccessToken, newRefreshToken) = await _tokenService.RefreshTokenAsync(
                request.RefreshToken,
                ipAddress,
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

            if (storedToken.UserType == AppUserType)
            {
                var user = await _userManager.FindByIdAsync(storedToken.OwnerId);
                if (user != null)
                {
                    email = user.Email;
                    userName = user.UserName;
                }
            }
            else if (storedToken.UserType == ClientUserType)
            {
                var clientRepo = _unitOfWork.Repository<Client>();
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

        public async Task LogoutAsync(string refreshToken, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return;

            await _tokenService.RevokeRefreshTokenAsync(refreshToken, ipAddress);
        }

        public async Task LogoutAllDevicesAsync(string ownerId, string userType, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ownerId) || string.IsNullOrWhiteSpace(userType))
                return;

            await _tokenService.RevokeAllUserRefreshTokensAsync(ownerId, userType, ipAddress);
        }

        public async Task LogoutOtherDevicesAsync(string ownerId, string userType, string currentDeviceId, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ownerId) ||
                string.IsNullOrWhiteSpace(userType) ||
                string.IsNullOrWhiteSpace(currentDeviceId))
                return;

            await _tokenService.RevokeOtherDeviceTokensAsync(ownerId, userType, currentDeviceId, ipAddress);
        }

        private DateTime GetAccessTokenExpirationUtc()
        {
            if (!double.TryParse(_configuration["JWT:AccessTokenDurationInMinutes"], out var minutes))
                throw new InvalidOperationException("Invalid JWT duration");

            return DateTime.UtcNow.AddMinutes(minutes);
        }

        private static string GenerateSecureToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }

        private static string ComputeSha256Hash(string input)
        {
            var normalized = input.Trim();
            var bytes = Encoding.UTF8.GetBytes(normalized);
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
