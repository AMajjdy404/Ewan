using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Booking;
using MediatR;

namespace Ewan.Application.Features.PropertyOwner.Bookings.Queries.GetPropertyBookings
{
    public record GetPropertyBookingsQuery(int RequesterPropertyId, PaginationParams Params)
        : IRequest<Pagination<BookingDetailsDto>>;
}
