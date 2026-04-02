using FluentValidation;

namespace Ewan.Application.Features.Client.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
            RuleFor(x => x.Request.PropertyId).GreaterThan(0);
            RuleFor(x => x.Request.RoomsCount).GreaterThan(0);
            RuleFor(x => x.Request.GuestsCount).GreaterThan(0);
            RuleFor(x => x.Request.CheckInDate)
                .Must(x => x.Date >= DateTime.UtcNow.Date)
                .WithMessage("Check-in date must be today or later.");
            RuleFor(x => x.Request)
                .Must(x => x.CheckOutDate.Date > x.CheckInDate.Date)
                .WithMessage("Check-out date must be after check-in date.");
        }
    }
}
