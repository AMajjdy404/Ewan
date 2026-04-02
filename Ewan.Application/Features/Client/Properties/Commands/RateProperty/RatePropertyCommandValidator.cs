using FluentValidation;

namespace Ewan.Application.Features.Client.Properties.Commands.RateProperty
{
    public class RatePropertyCommandValidator : AbstractValidator<RatePropertyCommand>
    {
        public RatePropertyCommandValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
            RuleFor(x => x.PropertyId).GreaterThan(0);
            RuleFor(x => x.Request.Rate).InclusiveBetween(1, 5);
            RuleFor(x => x.Request.Comment).MaximumLength(1000);
        }
    }
}
