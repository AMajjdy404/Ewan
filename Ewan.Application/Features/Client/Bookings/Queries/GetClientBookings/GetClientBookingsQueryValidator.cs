using FluentValidation;

namespace Ewan.Application.Features.Client.Bookings.Queries.GetClientBookings
{
    public class GetClientBookingsQueryValidator : AbstractValidator<GetClientBookingsQuery>
    {
        public GetClientBookingsQueryValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
        }
    }
}
