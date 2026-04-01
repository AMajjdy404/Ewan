namespace Ewan.Application.Features.Dashboard.Facilities.Commands.CreateFacility
{
    using FluentValidation;

    public class CreateFacilityCommandValidator : AbstractValidator<CreateFacilityCommand>
    {
        public CreateFacilityCommandValidator()
        {
            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
