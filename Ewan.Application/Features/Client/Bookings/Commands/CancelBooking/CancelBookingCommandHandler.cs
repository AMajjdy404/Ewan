using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ewan.Application.Features.Client.Bookings.Commands.CancelBooking
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelBookingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CancelBookingCommand command, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(command.BookingId);
            if (booking == null)
                throw new KeyNotFoundException("Booking not found.");

            if (booking.ClientId != command.ClientId)
                throw new UnauthorizedAccessException("You are not allowed to cancel this booking.");

            if (booking.Status == BookingStatus.Cancelled)
                throw new BadHttpRequestException("Booking is already cancelled.");

            if (booking.Status == BookingStatus.Completed)
                throw new BadHttpRequestException("Completed booking cannot be cancelled.");

            booking.Status = BookingStatus.Cancelled;
            booking.PaymentStatus = PaymentStatus.Failed;
            booking.CancelledAt = DateTime.UtcNow;
            booking.CancellationReason = command.Reason?.Trim();
            booking.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Booking>().Update(booking);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
