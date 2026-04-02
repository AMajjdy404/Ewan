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

            if (command.Request.RoomsCount > property.RoomCount)
                throw new BadHttpRequestException("Rooms count exceeds available rooms in the property.");

            var nightsCount = (command.Request.CheckOutDate.Date - command.Request.CheckInDate.Date).Days;
            var totalAmount = property.PricePerNight * nightsCount * command.Request.RoomsCount;

            var booking = new Booking
            {
                ClientId = command.ClientId,
                PropertyId = command.Request.PropertyId,
                CheckInDate = command.Request.CheckInDate.Date,
                CheckOutDate = command.Request.CheckOutDate.Date,
                NightsCount = nightsCount,
                RoomsCount = command.Request.RoomsCount,
                GuestsCount = command.Request.GuestsCount,
                PricePerNight = property.PricePerNight,
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
