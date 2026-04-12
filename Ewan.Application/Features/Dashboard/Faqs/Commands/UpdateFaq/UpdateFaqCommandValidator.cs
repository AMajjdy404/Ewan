using FluentValidation;

namespace Ewan.Application.Features.Dashboard.Faqs.Commands.UpdateFaq
{
    public class UpdateFaqCommandValidator : AbstractValidator<UpdateFaqCommand>
    {
        public UpdateFaqCommandValidator()
        {
            RuleFor(x => x.Request.Id).GreaterThan(0);
            RuleFor(x => x.Request.Question).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Request.Answer).NotEmpty().MaximumLength(4000);
        }
    }
}
