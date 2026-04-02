using Ewan.Core.Models.Dtos.Booking;
using MediatR;

namespace Ewan.Application.Features.Client.Bookings.Commands.CreateBooking
{
    public record CreateBookingCommand(int ClientId, CreateBookingRequestDto Request) : IRequest<int>;
}
