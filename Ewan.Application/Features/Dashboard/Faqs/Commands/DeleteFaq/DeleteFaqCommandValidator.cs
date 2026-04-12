using FluentValidation;

namespace Ewan.Application.Features.Dashboard.Faqs.Commands.DeleteFaq
{
    public class DeleteFaqCommandValidator : AbstractValidator<DeleteFaqCommand>
    {
        public DeleteFaqCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
