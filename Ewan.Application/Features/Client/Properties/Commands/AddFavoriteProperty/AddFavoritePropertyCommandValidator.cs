using FluentValidation;

namespace Ewan.Application.Features.Client.Properties.Commands.AddFavoriteProperty
{
    public class AddFavoritePropertyCommandValidator : AbstractValidator<AddFavoritePropertyCommand>
    {
        public AddFavoritePropertyCommandValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
            RuleFor(x => x.PropertyId).GreaterThan(0);
        }
    }
}
