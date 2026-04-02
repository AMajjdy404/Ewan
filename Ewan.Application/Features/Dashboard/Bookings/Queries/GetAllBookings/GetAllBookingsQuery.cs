using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Booking;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Bookings.Queries.GetAllBookings
{
    public record GetAllBookingsQuery(BookingFilterParams Params) : IRequest<DashboardBookingsResultDto>;
}
