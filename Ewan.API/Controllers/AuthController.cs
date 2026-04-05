using Ewan.API.Errors;
using Ewan.Application.Features.Auth.Commands.AppRefreshToken;
using Ewan.Application.Features.Auth.Commands.LoginAppUser;
using Ewan.Application.Features.Auth.Commands.LoginClient;
using Ewan.Application.Features.Auth.Commands.LoginPropertyOwner;
using Ewan.Application.Features.Auth.Commands.Logout;
using Ewan.Application.Features.Auth.Commands.LogoutAllDevices;
using Ewan.Application.Features.Auth.Commands.LogoutOtherDevices;
using Ewan.Application.Features.Auth.Commands.RegisterClient.Ewan.Application.Features.Auth.Commands.RegisterClient;
using Ewan.Application.Features.Auth.Commands.RequestClientPasswordReset;
using Ewan.Application.Features.Auth.Commands.ResetClientPassword;
using Ewan.Core.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var command = new LoginAppUserCommand(request, GetIpAddress());
            var result = await _mediator.Send(command);

            return Ok(new ApiResponse(200, "Login successful.", result));
        }

        [HttpPost("client-login")]
        public async Task<IActionResult> ClientLogin([FromBody] ClientLoginRequestDto request)
        {
            var command = new LoginClientCommand(request, GetIpAddress());
            var result = await _mediator.Send(command);

            return Ok(new ApiResponse(200, "Client login successful.", result));
        }

        [HttpPost("property-owner-login")]
        public async Task<IActionResult> PropertyOwnerLogin([FromBody] PropertyOwnerLoginRequestDto request)
        {
            var command = new LoginPropertyOwnerCommand(request);
            var result = await _mediator.Send(command);

            return Ok(new ApiResponse(200, "Property owner login successful.", result));
        }

        [HttpPost("client-register")]
        public async Task<IActionResult> ClientRegister([FromBody] RegisterClientDto request)
        {
            var command = new RegisterClientCommand(request, GetIpAddress());
            var result = await _mediator.Send(command);

            return Ok(new ApiResponse(200, "Client registered successfully.", result));
        }

        [HttpPost("client-forget-password")]
        public async Task<IActionResult> ClientForgetPassword([FromBody] ForgotClientPasswordDto request)
        {
            var command = new RequestClientPasswordResetCommand(request, GetIpAddress());
            await _mediator.Send(command);

            return Ok(new ApiResponse(200, "If the account exists, a reset email has been sent."));
        }

        [HttpPost("client-reset-password")]
        public async Task<IActionResult> ClientResetPassword([FromBody] ResetClientPasswordDto request)
        {
            var command = new ResetClientPasswordCommand(request, GetIpAddress());
            await _mediator.Send(command);

            return Ok(new ApiResponse(200, "Password reset successfully."));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var command = new RefreshTokenCommand(request, GetIpAddress());
            var result = await _mediator.Send(command);

            return Ok(new ApiResponse(200, "Token refreshed successfully.", result));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            var command = new LogoutCommand(request.RefreshToken, GetIpAddress());
            await _mediator.Send(command);

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

            var command = new LogoutAllDevicesCommand(userId, userType, GetIpAddress());
            await _mediator.Send(command);

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

            var command = new LogoutOtherDevicesCommand(userId, userType, deviceId, GetIpAddress());
            await _mediator.Send(command);

            return Ok(new ApiResponse(200, "Logged out from other devices."));
        }

        private string GetIpAddress()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
    }
}