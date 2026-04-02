using FluentValidation;

namespace Ewan.Application.Features.Client.Properties.Queries.GetAllClientProperties
{
    public class GetAllClientPropertiesQueryValidator : AbstractValidator<GetAllClientPropertiesQuery>
    {
        public GetAllClientPropertiesQueryValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
            RuleFor(x => x.Params.MinAverageRate)
                .InclusiveBetween(0, 5)
                .When(x => x.Params.MinAverageRate.HasValue);
        }
    }
}
