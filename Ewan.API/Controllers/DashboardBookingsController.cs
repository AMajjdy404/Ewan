using Ewan.API.Errors;
using Ewan.Application.Features.Dashboard.Bookings.Queries.GetAllBookings;
using Ewan.Application.Features.Dashboard.Bookings.Queries.GetBookingById;
using Ewan.Application.Features.Dashboard.Bookings.Queries.GetBookingStats;
using Ewan.Core.Models.Dtos.Booking;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/dashboard/bookings")]
    [Authorize]
    public class DashboardBookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardBookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BookingFilterParams param)
        {
            var result = await _mediator.Send(new GetAllBookingsQuery(param));
            return Ok(new ApiResponse(200, "Bookings retrieved successfully.", result));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetBookingByIdQuery(id));
            return Ok(new ApiResponse(200, "Booking retrieved successfully.", result));
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _mediator.Send(new GetBookingStatsQuery());
            return Ok(new ApiResponse(200, "Booking stats retrieved successfully.", result));
        }
    }
}
