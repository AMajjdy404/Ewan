using FluentValidation;

namespace Ewan.Application.Features.Client.Bookings.Commands.CancelBooking
{
    public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
    {
        public CancelBookingCommandValidator()
        {
            RuleFor(x => x.BookingId).GreaterThan(0);
            RuleFor(x => x.ClientId).GreaterThan(0);
            RuleFor(x => x.Reason)
                .MaximumLength(500);
        }
    }
}
