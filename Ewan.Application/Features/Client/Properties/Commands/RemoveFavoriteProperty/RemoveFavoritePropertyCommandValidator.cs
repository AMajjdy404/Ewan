using FluentValidation;

namespace Ewan.Application.Features.Client.Properties.Commands.RemoveFavoriteProperty
{
    public class RemoveFavoritePropertyCommandValidator : AbstractValidator<RemoveFavoritePropertyCommand>
    {
        public RemoveFavoritePropertyCommandValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
            RuleFor(x => x.PropertyId).GreaterThan(0);
        }
    }
}
