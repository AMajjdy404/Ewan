using Ewan.Core.Models;
using Ewan.Core.Services;
using Ewan.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Ewan.Application
{
    public class TokenService : ITokenService
    {
        private const string AppUserType = "AppUser";
        private const string ClientUserType = "Client";
        private const int MaxActiveSessionsPerUser = 5;

        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(
            IConfiguration configuration,
            AppDbContext db,
            UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _db = db;
            _userManager = userManager;
        }

        public async Task<(string accessToken, string refreshTokenPlain)> CreateTokenAsync(
            AppUser appUser,
            string ipAddress,
            string? deviceInfo = null,
            string? deviceId = null,
            bool rememberMe = false)
        {
            var claims = await BuildClaimsForAppUserAsync(appUser);
            var accessToken = GenerateJwtToken(claims);

            await EnforceSessionLimitAsync(appUser.Id.ToString(), AppUserType, ipAddress);

            var (plainRefreshToken, refreshTokenEntity) = GenerateRefreshTokenEntity(
                ownerId: appUser.Id.ToString(),
                userType: AppUserType,
                ipAddress: ipAddress,
                deviceInfo: deviceInfo,
                deviceId: deviceId,
                rememberMe: rememberMe);

            await _db.RefreshTokens.AddAsync(refreshTokenEntity);
            await _db.SaveChangesAsync();

            return (accessToken, plainRefreshToken);
        }

        public async Task<(string accessToken, string refreshTokenPlain)> CreateTokenAsync(
            Client client,
            string ipAddress,
            string? deviceInfo = null,
            string? deviceId = null,
            bool rememberMe = false)
        {
            var claims = BuildClaimsForClient(client);
            var accessToken = GenerateJwtToken(claims);

            await EnforceSessionLimitAsync(client.Id.ToString(), ClientUserType, ipAddress);

            var (plainRefreshToken, refreshTokenEntity) = GenerateRefreshTokenEntity(
                ownerId: client.Id.ToString(),
                userType: ClientUserType,
                ipAddress: ipAddress,
                deviceInfo: deviceInfo,
                deviceId: deviceId,
                rememberMe: rememberMe);

            await _db.RefreshTokens.AddAsync(refreshTokenEntity);
            await _db.SaveChangesAsync();

            return (accessToken, plainRefreshToken);
        }

        public async Task<(string newAccessToken, string newRefreshTokenPlain)> RefreshTokenAsync(
            string refreshTokenPlain,
            string ipAddress,
            string? deviceInfo = null,
            string? deviceId = null)
        {
            var normalizedToken = NormalizeToken(refreshTokenPlain);
            if (string.IsNullOrWhiteSpace(normalizedToken))
                throw new SecurityTokenException("Invalid refresh token");

            var tokenHash = ComputeSha256Hash(normalizedToken);

            var existing = await _db.RefreshTokens
                .FirstOrDefaultAsync(r => r.TokenHash == tokenHash);

            if (existing == null)
                throw new SecurityTokenException("Invalid refresh token");

            // reuse detection
            if (existing.IsRevoked || existing.IsCompromised)
            {
                await MarkTokenFamilyAsCompromisedAsync(
                    existing.OwnerId,
                    existing.UserType,
                    ipAddress,
                    "Refresh token reuse detected");

                throw new SecurityTokenException("Refresh token reuse detected");
            }

            if (existing.IsExpired)
                throw new SecurityTokenException("Refresh token expired");

            // device binding
            if (!string.IsNullOrWhiteSpace(existing.DeviceId) &&
                !string.IsNullOrWhiteSpace(deviceId) &&
                !string.Equals(existing.DeviceId, deviceId, StringComparison.Ordinal))
            {
                await MarkTokenFamilyAsCompromisedAsync(
                    existing.OwnerId,
                    existing.UserType,
                    ipAddress,
                    "Device mismatch detected");

                throw new SecurityTokenException("Device mismatch detected");
            }

            var (newPlainToken, newEntity) = GenerateRefreshTokenEntity(
                ownerId: existing.OwnerId,
                userType: existing.UserType,
                ipAddress: ipAddress,
                deviceInfo: deviceInfo,
                deviceId: deviceId ?? existing.DeviceId,
                rememberMe: existing.IsRememberMe);

            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;
            existing.RevokedByIpAddress = ipAddress;
            existing.ReplacedByTokenHash = newEntity.TokenHash;

            _db.RefreshTokens.Update(existing);
            await _db.RefreshTokens.AddAsync(newEntity);

            var claims = await BuildClaimsFromRefreshTokenOwnerAsync(existing);
            var newAccessToken = GenerateJwtToken(claims);

            await _db.SaveChangesAsync();

            return (newAccessToken, newPlainToken);
        }

        public async Task RevokeRefreshTokenAsync(string refreshTokenPlain, string ipAddress)
        {
            var normalizedToken = NormalizeToken(refreshTokenPlain);
            if (string.IsNullOrWhiteSpace(normalizedToken))
                return;

            var tokenHash = ComputeSha256Hash(normalizedToken);

            var existing = await _db.RefreshTokens
                .FirstOrDefaultAsync(r => r.TokenHash == tokenHash);

            if (existing == null || existing.IsRevoked)
                return;

            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;
            existing.RevokedByIpAddress = ipAddress;

            _db.RefreshTokens.Update(existing);
            await _db.SaveChangesAsync();
        }

        public async Task RevokeAllUserRefreshTokensAsync(
            string ownerId,
            string userType,
            string ipAddress,
            string? reason = null)
        {
            var activeTokens = await _db.RefreshTokens
                .Where(x => x.OwnerId == ownerId &&
                            x.UserType == userType &&
                            !x.IsRevoked &&
                            !x.IsCompromised &&
                            x.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                token.RevokedByIpAddress = ipAddress;
                token.CompromisedReason = reason;
            }

            _db.RefreshTokens.UpdateRange(activeTokens);
            await _db.SaveChangesAsync();
        }

        public async Task RevokeOtherDeviceTokensAsync(
            string ownerId,
            string userType,
            string currentDeviceId,
            string ipAddress)
        {
            var otherTokens = await _db.RefreshTokens
                .Where(x => x.OwnerId == ownerId &&
                            x.UserType == userType &&
                            x.DeviceId != currentDeviceId &&
                            !x.IsRevoked &&
                            !x.IsCompromised &&
                            x.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in otherTokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                token.RevokedByIpAddress = ipAddress;
            }

            _db.RefreshTokens.UpdateRange(otherTokens);
            await _db.SaveChangesAsync();
        }

        private async Task<List<Claim>> BuildClaimsForAppUserAsync(AppUser appUser)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            new Claim(ClaimTypes.Email, appUser.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, appUser.UserName ?? string.Empty),
            new Claim("UserType", AppUserType)
        };

            var roles = await _userManager.GetRolesAsync(appUser);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            return claims;
        }

        private List<Claim> BuildClaimsForClient(Client client)
        {
            return new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
            new Claim(ClaimTypes.Email, client.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, client.FullName ?? string.Empty),
            new Claim(ClaimTypes.MobilePhone, client.PhoneNumber ?? string.Empty),
            new Claim("UserType", ClientUserType)
        };
        }

        private async Task<List<Claim>> BuildClaimsFromRefreshTokenOwnerAsync(RefreshToken token)
        {
            if (token.UserType == AppUserType)
            {
                var user = await _userManager.FindByIdAsync(token.OwnerId);
                if (user == null)
                    throw new SecurityTokenException("Invalid token owner");

                return await BuildClaimsForAppUserAsync(user);
            }

            if (token.UserType == ClientUserType)
            {
                if (!int.TryParse(token.OwnerId, out var clientId))
                    throw new SecurityTokenException("Invalid token owner");

                var client = await _db.Clients.FindAsync(clientId);
                if (client == null)
                    throw new SecurityTokenException("Invalid token owner");

                return BuildClaimsForClient(client);
            }

            throw new SecurityTokenException("Invalid user type");
        }

        private string GenerateJwtToken(List<Claim> claims)
        {
            var key = _configuration["JWT:Key"];
            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("JWT Key missing");

            if (!double.TryParse(_configuration["JWT:AccessTokenDurationInMinutes"], out var minutes))
                throw new InvalidOperationException("Invalid JWT duration");

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(minutes),
                claims: claims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private (string plainToken, RefreshToken entity) GenerateRefreshTokenEntity(
            string ownerId,
            string userType,
            string ipAddress,
            string? deviceInfo,
            string? deviceId,
            bool rememberMe)
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            var plainToken = NormalizeToken(Convert.ToBase64String(bytes));
            var hash = ComputeSha256Hash(plainToken);

            var normal = _configuration["JWT:RefreshTokenDurationInDays"];
            var remember = _configuration["JWT:RefreshTokenRememberMeDurationInDays"];
            var selected = rememberMe ? remember : normal;

            if (!double.TryParse(selected, out var days))
                throw new InvalidOperationException("Invalid refresh duration");

            var entity = new RefreshToken
            {
                TokenHash = hash,
                OwnerId = ownerId,
                UserType = userType,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(days),
                RemoteIpAddress = ipAddress,
                DeviceInfo = deviceInfo,
                DeviceId = deviceId,
                IsRememberMe = rememberMe
            };

            return (plainToken, entity);
        }

        private async Task EnforceSessionLimitAsync(string ownerId, string userType, string ipAddress)
        {
            var activeTokens = await _db.RefreshTokens
                .Where(x => x.OwnerId == ownerId &&
                            x.UserType == userType &&
                            !x.IsRevoked &&
                            !x.IsCompromised &&
                            x.ExpiresAt > DateTime.UtcNow)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            var extraCount = (activeTokens.Count + 1) - MaxActiveSessionsPerUser;
            if (extraCount <= 0)
                return;

            var tokensToRevoke = activeTokens.Take(extraCount).ToList();

            foreach (var token in tokensToRevoke)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                token.RevokedByIpAddress = ipAddress;
            }

            _db.RefreshTokens.UpdateRange(tokensToRevoke);
        }

        private async Task MarkTokenFamilyAsCompromisedAsync(
            string ownerId,
            string userType,
            string ipAddress,
            string reason)
        {
            var relatedTokens = await _db.RefreshTokens
                .Where(x => x.OwnerId == ownerId &&
                            x.UserType == userType &&
                            !x.IsCompromised &&
                            x.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in relatedTokens)
            {
                token.IsCompromised = true;
                token.CompromisedAt = DateTime.UtcNow;
                token.CompromisedReason = reason;

                if (!token.IsRevoked)
                {
                    token.IsRevoked = true;
                    token.RevokedAt = DateTime.UtcNow;
                    token.RevokedByIpAddress = ipAddress;
                }
            }

            _db.RefreshTokens.UpdateRange(relatedTokens);
            await _db.SaveChangesAsync();
        }

        private static string ComputeSha256Hash(string input)
        {
            var normalized = NormalizeToken(input);
            var bytes = Encoding.UTF8.GetBytes(normalized);
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }

        private static string NormalizeToken(string? token)
        {
            return token?.Trim().TrimEnd('=') ?? string.Empty;
        }
    }
}
