using Ewan.API.Errors;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAppUserAsync(request, GetIpAddress());
            return Ok(new ApiResponse(200, "Login successful.", result));
        }

        [HttpPost("client-login")]
        public async Task<IActionResult> ClientLogin([FromBody] ClientLoginRequestDto request)
        {
            var result = await _authService.LoginClientAsync(request, GetIpAddress());
            return Ok(new ApiResponse(200, "Client login successful.", result));
        }

        [HttpPost("client-register")]
        public async Task<IActionResult> ClientRegister([FromBody] RegisterClientDto request)
        {
            var result = await _authService.RegisterClientAsync(request, GetIpAddress());
            return Ok(new ApiResponse(200, "Client registered successfully.", result));
        }

        [HttpPost("client-forgot-password")]
        public async Task<IActionResult> ClientForgotPassword([FromBody] ForgotClientPasswordDto request)
        {
            await _authService.RequestClientPasswordResetAsync(request, GetIpAddress());

            return Ok(new ApiResponse(
                200,
                "If the account exists, a reset email has been sent."));
        }

        [HttpPost("client-reset-password")]
        public async Task<IActionResult> ClientResetPassword([FromBody] ResetClientPasswordDto request)
        {
            await _authService.ResetClientPasswordAsync(request, GetIpAddress());

            return Ok(new ApiResponse(200, "Password reset successfully."));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokenAsync(request, GetIpAddress());
            return Ok(new ApiResponse(200, "Token refreshed successfully.", result));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            await _authService.LogoutAsync(request.RefreshToken, GetIpAddress());

            return Ok(new ApiResponse(200, "Logged out successfully."));
        }

        [Authorize]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAllDevices()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userType = User.FindFirstValue("UserType");

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(userType))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            await _authService.LogoutAllDevicesAsync(userId, userType, GetIpAddress());

            return Ok(new ApiResponse(200, "Logged out from all devices."));
        }

        [Authorize]
        [HttpPost("logout-others")]
        public async Task<IActionResult> LogoutOtherDevices([FromQuery] string deviceId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userType = User.FindFirstValue("UserType");

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(userType))
                return Unauthorized(new ApiResponse(401, "Unauthorized"));

            if (string.IsNullOrWhiteSpace(deviceId))
                return BadRequest(new ApiResponse(400, "DeviceId is required."));

            await _authService.LogoutOtherDevicesAsync(userId, userType, deviceId, GetIpAddress());

            return Ok(new ApiResponse(200, "Logged out from other devices."));
        }

        private string GetIpAddress()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
    }
}