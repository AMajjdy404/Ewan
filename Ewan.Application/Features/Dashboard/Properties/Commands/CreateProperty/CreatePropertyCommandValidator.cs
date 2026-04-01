using FluentValidation;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
            RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Request.GroupId).GreaterThan(0);
            RuleFor(x => x.Request.Address).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Request.Location).NotEmpty();
            RuleFor(x => x.Request.PricePerNight).GreaterThan(0);
            RuleFor(x => x.Request.RoomCount).GreaterThan(0);
            RuleFor(x => x.Request.GuestCount).GreaterThan(0);
        }
    }
}
