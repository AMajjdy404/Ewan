using Ewan.API.Errors;
using Ewan.Application.Features.Client.Properties.Commands.AddFavoriteProperty;
using Ewan.Application.Features.Client.Properties.Commands.RateProperty;
using Ewan.Application.Features.Client.Properties.Commands.RemoveFavoriteProperty;
using Ewan.Application.Features.Client.Properties.Queries.GetAllClientProperties;
using Ewan.Application.Features.Client.Properties.Queries.GetPropertyRatings;
using Ewan.Core.Models.Dtos.Property;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/client/properties")]
    [Authorize]
    public class ClientPropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientPropertiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ClientPropertyFilterParams param)
        {
            var clientId = GetClientId();
            var result = await _mediator.Send(new GetAllClientPropertiesQuery(clientId, param));
            return Ok(new ApiResponse(200, "Properties retrieved successfully.", result));
        }

        [HttpPost("{propertyId:int}/favorite")]
        public async Task<IActionResult> AddToFavorite(int propertyId)
        {
            var clientId = GetClientId();
            await _mediator.Send(new AddFavoritePropertyCommand(clientId, propertyId));
            return Ok(new ApiResponse(200, "Property added to favorites successfully."));
        }

        [HttpDelete("{propertyId:int}/favorite")]
        public async Task<IActionResult> RemoveFromFavorite(int propertyId)
        {
            var clientId = GetClientId();
            await _mediator.Send(new RemoveFavoritePropertyCommand(clientId, propertyId));
            return Ok(new ApiResponse(200, "Property removed from favorites successfully."));
        }

        [HttpPost("{propertyId:int}/rate")]
        public async Task<IActionResult> Rate(int propertyId, [FromBody] RatePropertyRequestDto request)
        {
            var clientId = GetClientId();
            await _mediator.Send(new RatePropertyCommand(clientId, propertyId, request));
            return Ok(new ApiResponse(200, "Property rated successfully."));
        }

        [HttpGet("{propertyId:int}/ratings")]
        public async Task<IActionResult> GetRatings(int propertyId)
        {
            var result = await _mediator.Send(new GetPropertyRatingsQuery(propertyId));
            return Ok(new ApiResponse(200, "Property ratings retrieved successfully.", result));
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
