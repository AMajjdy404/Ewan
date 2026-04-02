using FluentValidation;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyCommandValidator()
        {
            RuleFor(x => x.Request.Id).GreaterThan(0);
            RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Request.Description).NotEmpty().MaximumLength(2000);
            RuleFor(x => x.Request.GroupId).GreaterThan(0);
            RuleFor(x => x.Request.Address).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Request.Location).NotEmpty();
            RuleFor(x => x.Request.PricePerNight).GreaterThan(0);
            RuleFor(x => x.Request.RoomCount).GreaterThan(0);
            RuleFor(x => x.Request.GuestCount).GreaterThan(0);
        }
    }
}
