using FluentValidation;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
            RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Request.Description).NotEmpty().MaximumLength(2000);
            RuleFor(x => x.Request.OwnerPhoneNumber).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Request.OwnerPassword).NotEmpty().MinimumLength(6).MaximumLength(100);
            RuleFor(x => x.Request.GroupId).GreaterThan(0);
            RuleFor(x => x.Request.Address).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Request.Location).NotEmpty();
            RuleFor(x => x.Request.PricePerNight).GreaterThan(0);
            RuleFor(x => x.Request.RoomCount).GreaterThan(0);
            RuleFor(x => x.Request.GuestCount).GreaterThan(0);
            RuleForEach(x => x.Request.FacilityIds).GreaterThan(0);
        }
    }
}
