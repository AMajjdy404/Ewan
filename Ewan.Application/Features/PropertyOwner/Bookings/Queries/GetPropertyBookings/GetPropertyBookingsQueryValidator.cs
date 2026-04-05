using FluentValidation;

namespace Ewan.Application.Features.PropertyOwner.Bookings.Queries.GetPropertyBookings
{
    public class GetPropertyBookingsQueryValidator : AbstractValidator<GetPropertyBookingsQuery>
    {
        public GetPropertyBookingsQueryValidator()
        {
            RuleFor(x => x.RequesterPropertyId).GreaterThan(0);
        }
    }
}
