using FluentValidation;

namespace Ewan.Application.Features.Client.Profile.Queries.GetClientProfile
{
    public class GetClientProfileQueryValidator : AbstractValidator<GetClientProfileQuery>
    {
        public GetClientProfileQueryValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
        }
    }
}
