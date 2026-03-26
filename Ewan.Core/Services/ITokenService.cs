using Ewan.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Ewan.Core.Services
{
    public interface ITokenService
    {
        Task<(string accessToken, string refreshTokenPlain)> CreateTokenAsync(
            AppUser appUser,
            string ipAddress,
            string? deviceInfo = null,
            string? deviceId = null,
            bool rememberMe = false);

        Task<(string accessToken, string refreshTokenPlain)> CreateTokenAsync(
            Client client,
            string ipAddress,
            string? deviceInfo = null,
            string? deviceId = null,
            bool rememberMe = false);

        Task<(string newAccessToken, string newRefreshTokenPlain)> RefreshTokenAsync(
            string refreshTokenPlain,
            string ipAddress,
            string? deviceInfo = null,
            string? deviceId = null);

        Task RevokeRefreshTokenAsync(
            string refreshTokenPlain,
            string ipAddress);

        Task RevokeAllUserRefreshTokensAsync(
            string ownerId,
            string userType,
            string ipAddress,
            string? reason = null);

        Task RevokeOtherDeviceTokensAsync(
            string ownerId,
            string userType,
            string currentDeviceId,
            string ipAddress);
    }
}
