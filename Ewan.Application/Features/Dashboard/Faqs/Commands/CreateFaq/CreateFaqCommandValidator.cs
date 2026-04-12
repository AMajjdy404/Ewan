using FluentValidation;

namespace Ewan.Application.Features.Dashboard.Faqs.Commands.CreateFaq
{
    public class CreateFaqCommandValidator : AbstractValidator<CreateFaqCommand>
    {
        public CreateFaqCommandValidator()
        {
            RuleFor(x => x.Request.Question).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Request.Answer).NotEmpty().MaximumLength(4000);
        }
    }
}
