using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Booking;
using MediatR;

namespace Ewan.Application.Features.Client.Bookings.Queries.GetClientBookings
{
    public record GetClientBookingsQuery(int ClientId, ClientBookingFilterParams Params) : IRequest<Pagination<ClientBookingDto>>;
}
