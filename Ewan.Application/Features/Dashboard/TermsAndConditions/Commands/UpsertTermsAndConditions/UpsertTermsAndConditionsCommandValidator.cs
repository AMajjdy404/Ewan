using FluentValidation;

namespace Ewan.Application.Features.Dashboard.TermsAndConditions.Commands.UpsertTermsAndConditions
{
    public class UpsertTermsAndConditionsCommandValidator : AbstractValidator<UpsertTermsAndConditionsCommand>
    {
        public UpsertTermsAndConditionsCommandValidator()
        {
            RuleFor(x => x.Request.Content)
                .NotEmpty()
                .MaximumLength(10000);
        }
    }
}
