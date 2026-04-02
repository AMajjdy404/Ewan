using Ewan.Core.Models.Dtos.Booking;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Bookings.Queries.GetBookingStats
{
    public record GetBookingStatsQuery : IRequest<BookingStatsDto>;
}
