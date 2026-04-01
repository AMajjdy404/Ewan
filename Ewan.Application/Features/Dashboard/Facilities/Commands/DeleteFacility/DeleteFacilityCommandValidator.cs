namespace Ewan.Application.Features.Dashboard.Facilities.Commands.DeleteFacility
{
    using FluentValidation;

    public class DeleteFacilityCommandValidator : AbstractValidator<DeleteFacilityCommand>
    {
        public DeleteFacilityCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
