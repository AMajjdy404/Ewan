using Ewan.Application.Helpers;
using Ewan.Application.Helpers;
using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ewan.Application.Features.Client.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
        {
            var property = await _unitOfWork.Repository<Property>().GetByIdAsync(command.Request.PropertyId);
            if (property == null)
                throw new KeyNotFoundException("Property not found.");

            if (!property.IsAvailable)
                throw new BadHttpRequestException("Property is not available.");

            if (command.Request.GuestsCount > property.GuestCount)
                throw new BadHttpRequestException("Guests count exceeds property capacity.");

            var bookingMode = property.BookingMode;
            if (bookingMode == 0 && property.PropertyType != 0)
                bookingMode = PropertyBookingModeResolver.ResolveFromPropertyType(property.PropertyType);

            if (bookingMode == BookingMode.RoomBased)
            {
                if (command.Request.RoomsCount <= 0)
                    throw new BadHttpRequestException("Rooms count must be greater than zero for hotel bookings.");

                if (command.Request.RoomsCount > property.RoomCount)
                    throw new BadHttpRequestException("Rooms count exceeds available rooms in the property.");
            }

            var requestedStart = command.Request.CheckInDate;
            var requestedEnd = command.Request.CheckOutDate;

            var bookings = await _unitOfWork.Repository<Booking>().ListAllAsync();
            var overlappingBookings = bookings
                .Where(x =>
                    x.PropertyId == property.Id &&
                    x.Status != BookingStatus.Cancelled &&
                    requestedStart < x.CheckOutDate &&
                    x.CheckInDate < requestedEnd)
                .ToList();

            switch (bookingMode)
            {
                case BookingMode.ExclusiveStay:
                    if (overlappingBookings.Count > 0)
                        throw new BadHttpRequestException("This property is fully reserved for the selected period.");
                    break;

                case BookingMode.RoomBased:
                    var reservedRooms = overlappingBookings.Sum(x => x.RoomsCount);
                    if (reservedRooms + command.Request.RoomsCount > property.RoomCount)
                        throw new BadHttpRequestException("Not enough available rooms for the selected period.");
                    break;

                case BookingMode.TimeSlot:
                    if (overlappingBookings.Count > 0)
                        throw new BadHttpRequestException("This time slot is not available for the selected period.");
                    break;

                default:
                    throw new InvalidOperationException("Property booking mode is not configured correctly.");
            }

            var nightsCount = (command.Request.CheckOutDate.Date - command.Request.CheckInDate.Date).Days;
            if (nightsCount <= 0)
                nightsCount = 1;

            var roomsCount = bookingMode == BookingMode.RoomBased ? command.Request.RoomsCount : 0;
            var billedUnits = bookingMode == BookingMode.RoomBased ? roomsCount : 1;
            var rate = bookingMode == BookingMode.TimeSlot ? property.PricePerHour : property.PricePerNight;
            var durationFactor = bookingMode == BookingMode.TimeSlot
                ? Math.Max(1, (decimal)Math.Ceiling((command.Request.CheckOutDate - command.Request.CheckInDate).TotalHours))
                : nightsCount;

            var totalAmount = rate * durationFactor * billedUnits;

            var booking = new Booking
            {
                ClientId = command.ClientId,
                PropertyId = command.Request.PropertyId,
                CheckInDate = command.Request.CheckInDate,
                CheckOutDate = command.Request.CheckOutDate,
                NightsCount = nightsCount,
                RoomsCount = roomsCount,
                GuestsCount = command.Request.GuestsCount,
                PricePerNight = rate,
                TotalAmount = totalAmount,
                Status = BookingStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                PaymentMethod = command.Request.PaymentMethod,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Booking>().AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return booking.Id;
        }
    }
}
