using FluentValidation;

namespace Ewan.Application.Features.Dashboard.Bookings.Queries.GetAllBookings
{
    public class GetAllBookingsQueryValidator : AbstractValidator<GetAllBookingsQuery>
    {
        public GetAllBookingsQueryValidator()
        {
            RuleFor(x => x.Params)
                .Must(x => !x.FromDate.HasValue || !x.ToDate.HasValue || x.FromDate.Value.Date <= x.ToDate.Value.Date)
                .WithMessage("FromDate must be earlier than or equal to ToDate.");
        }
    }
}
