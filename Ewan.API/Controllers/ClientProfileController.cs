using Ewan.API.Errors;
using Ewan.Application.Features.Client.Profile.Queries.GetClientProfile;
using Ewan.Application.Features.Client.Profile.Commands.DeleteClientAccount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/client/profile")]
    [Authorize]
    public class ClientProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var clientId = GetClientId();
            var result = await _mediator.Send(new GetClientProfileQuery(clientId));
            return Ok(new ApiResponse(200, "Client profile retrieved successfully.", result));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var clientId = GetClientId();
            await _mediator.Send(new DeleteClientAccountCommand(clientId));
            return Ok(new ApiResponse(200, "Client account deleted successfully."));
        }

        private int GetClientId()
        {
            var userType = User.FindFirstValue("UserType");
            if (!string.Equals(userType, "Client", StringComparison.Ordinal))
                throw new UnauthorizedAccessException("Only clients can access this endpoint.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userId, out var clientId))
                throw new UnauthorizedAccessException("Invalid client token.");

            return clientId;
        }
    }
}
