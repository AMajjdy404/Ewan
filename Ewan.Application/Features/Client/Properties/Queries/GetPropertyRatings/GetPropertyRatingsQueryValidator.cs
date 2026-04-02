using FluentValidation;

namespace Ewan.Application.Features.Client.Properties.Queries.GetPropertyRatings
{
    public class GetPropertyRatingsQueryValidator : AbstractValidator<GetPropertyRatingsQuery>
    {
        public GetPropertyRatingsQueryValidator()
        {
            RuleFor(x => x.PropertyId).GreaterThan(0);
        }
    }
}
