using Ewan.API.Errors;
using Ewan.Application.Features.Client.Bookings.Commands.CancelBooking;
using Ewan.Application.Features.Client.Bookings.Commands.CreateBooking;
using Ewan.Application.Features.Client.Bookings.Queries.GetClientBookings;
using Ewan.Core.Models.Dtos.Booking;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/client/bookings")]
    [Authorize]
    public class ClientBookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientBookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ClientBookingFilterParams param)
        {
            var clientId = GetClientId();
            var result = await _mediator.Send(new GetClientBookingsQuery(clientId, param));
            return Ok(new ApiResponse(200, "Client bookings retrieved successfully.", result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingRequestDto request)
        {
            var clientId = GetClientId();
            var id = await _mediator.Send(new CreateBookingCommand(clientId, request));
            return Ok(new ApiResponse(201, "Booking created successfully.", id));
        }

        [HttpPatch("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id, [FromBody] CancelBookingRequestDto request)
        {
            var clientId = GetClientId();
            await _mediator.Send(new CancelBookingCommand(id, clientId, request.Reason));
            return Ok(new ApiResponse(200, "Booking cancelled successfully."));
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
