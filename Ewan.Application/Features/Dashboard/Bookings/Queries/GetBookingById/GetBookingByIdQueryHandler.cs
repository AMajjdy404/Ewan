using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Booking;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Bookings.Queries.GetBookingById
{
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDetailsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBookingByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BookingDetailsDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new BookingWithDetailsSpecification(request.Id);
            var booking = await _unitOfWork.Repository<Booking>().GetEntityWithSpec(spec);

            if (booking == null)
                throw new KeyNotFoundException("Booking not found.");

            return new BookingDetailsDto
            {
                Id = booking.Id,
                ClientId = booking.ClientId,
                ClientName = booking.Client.FullName,
                ClientEmail = booking.Client.Email,
                ClientPhoneNumber = booking.Client.PhoneNumber,
                PropertyId = booking.PropertyId,
                PropertyName = booking.Property.Name,
                PropertyAddress = booking.Property.Address,
                PropertyLocation = booking.Property.Location,
                PropertyRoomCount = booking.Property.RoomCount,
                PropertyGuestCount = booking.Property.GuestCount,
                PropertyImageUrls = booking.Property.Images.Select(x => x.ImageUrl).ToList(),
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                NightsCount = booking.NightsCount,
                RoomsCount = booking.RoomsCount,
                GuestsCount = booking.GuestsCount,
                PricePerNight = booking.PricePerNight,
                TotalAmount = booking.TotalAmount,
                Status = booking.Status,
                PaymentStatus = booking.PaymentStatus,
                PaymentMethod = booking.PaymentMethod,
                CreatedAt = booking.CreatedAt,
                CancelledAt = booking.CancelledAt,
                CancellationReason = booking.CancellationReason
            };
        }
    }
}
