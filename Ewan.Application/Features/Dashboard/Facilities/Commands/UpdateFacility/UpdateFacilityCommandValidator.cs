namespace Ewan.Application.Features.Dashboard.Facilities.Commands.UpdateFacility
{
    using FluentValidation;

    public class UpdateFacilityCommandValidator : AbstractValidator<UpdateFacilityCommand>
    {
        public UpdateFacilityCommandValidator()
        {
            RuleFor(x => x.Request.Id)
                .GreaterThan(0);

            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
