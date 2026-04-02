using Ewan.Core.Models.Dtos.Booking;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Bookings.Queries.GetBookingById
{
    public record GetBookingByIdQuery(int Id) : IRequest<BookingDetailsDto>;
}
