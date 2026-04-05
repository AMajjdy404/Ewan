using Ewan.API.Errors;
using Ewan.Application.Features.PropertyOwner.Bookings.Queries.GetPropertyBookings;
using Ewan.Core.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/property-owner/bookings")]
    [Authorize]
    public class PropertyOwnerBookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertyOwnerBookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams param)
        {
            var requesterPropertyId = GetRequesterPropertyId();
            var result = await _mediator.Send(new GetPropertyBookingsQuery(requesterPropertyId, param));
            return Ok(new ApiResponse(200, "Property bookings retrieved successfully.", result));
        }

        private int GetRequesterPropertyId()
        {
            var userType = User.FindFirstValue("UserType");
            if (!string.Equals(userType, "PropertyOwner", StringComparison.Ordinal))
                throw new UnauthorizedAccessException("Only property owners can access this endpoint.");

            var propertyIdClaim = User.FindFirstValue("PropertyId");
            if (!int.TryParse(propertyIdClaim, out var propertyId))
                throw new UnauthorizedAccessException("Invalid property owner token.");

            return propertyId;
        }
    }
}
