using MediatR;

namespace Ewan.Application.Features.Client.Bookings.Commands.CancelBooking
{
    public record CancelBookingCommand(int BookingId, int ClientId, string? Reason) : IRequest;
}
