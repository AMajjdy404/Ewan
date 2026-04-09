using FluentValidation;

namespace Ewan.Application.Features.Client.Properties.Queries.GetFavouriteClientProperties
{
    public class GetFavouriteClientPropertiesQueryValidator : AbstractValidator<GetFavouriteClientPropertiesQuery>
    {
        public GetFavouriteClientPropertiesQueryValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
        }
    }
}
