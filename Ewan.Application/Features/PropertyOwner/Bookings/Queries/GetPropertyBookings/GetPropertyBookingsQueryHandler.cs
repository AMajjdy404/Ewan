using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Booking;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Ewan.Application.Features.PropertyOwner.Bookings.Queries.GetPropertyBookings
{
    public class GetPropertyBookingsQueryHandler : IRequestHandler<GetPropertyBookingsQuery, Pagination<BookingDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _baseApiUrl;

        public GetPropertyBookingsQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _baseApiUrl = configuration["BaseApiUrl"] ?? string.Empty;
        }

        public async Task<Pagination<BookingDetailsDto>> Handle(GetPropertyBookingsQuery request, CancellationToken cancellationToken)
        {
            var propertyId = request.RequesterPropertyId;

            var propertyExists = await _unitOfWork.Repository<Property>().AnyAsync(x => x.Id == propertyId);
            if (!propertyExists)
                throw new KeyNotFoundException("Property not found.");

            var spec = new PropertyBookingsWithDetailsSpecification(propertyId, request.Params);
            var countSpec = new PropertyBookingsCountSpecification(propertyId);

            var bookingRepo = _unitOfWork.Repository<Booking>();
            var bookings = await bookingRepo.ListAsync(spec);
            var totalCount = await bookingRepo.CountAsync(countSpec);

            var data = bookings.Select(booking => new BookingDetailsDto
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
                PropertyImageUrls = booking.Property.Images.Select(x => ToAbsoluteImageUrl(x.ImageUrl)).ToList(),
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
            }).ToList();

            return new Pagination<BookingDetailsDto>(
                request.Params.PageIndex,
                request.Params.PageSize,
                totalCount,
                data);
        }

        private string ToAbsoluteImageUrl(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return imageUrl;

            if (Uri.TryCreate(imageUrl, UriKind.Absolute, out _))
                return imageUrl;

            if (string.IsNullOrWhiteSpace(_baseApiUrl))
                return imageUrl;

            return $"{_baseApiUrl.TrimEnd('/')}/{imageUrl.TrimStart('/')}";
        }
    }
}
