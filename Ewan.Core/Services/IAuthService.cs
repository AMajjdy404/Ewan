using Ewan.Core.Models.Dtos;

namespace Ewan.Core.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAppUserAsync(LoginRequestDto request, string ipAddress);

        Task<AuthResponseDto> LoginClientAsync(ClientLoginRequestDto request, string ipAddress);

        Task<AuthResponseDto> RegisterClientAsync(RegisterClientDto request, string ipAddress);

        Task RequestClientPasswordResetAsync(ForgotClientPasswordDto request, string ipAddress);

        Task ResetClientPasswordAsync(ResetClientPasswordDto request, string ipAddress);

        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, string ipAddress);

        Task LogoutAsync(string refreshToken, string ipAddress);

        Task LogoutAllDevicesAsync(string ownerId, string userType, string ipAddress);

        Task LogoutOtherDevicesAsync(string ownerId, string userType, string currentDeviceId, string ipAddress);
    }
}
