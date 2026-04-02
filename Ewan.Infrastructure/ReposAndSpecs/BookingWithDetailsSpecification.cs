using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Booking;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class BookingWithDetailsSpecification : BaseSpecification<Booking>
    {
        public BookingWithDetailsSpecification(BookingFilterParams filter)
            : base(x =>
                (string.IsNullOrWhiteSpace(filter.Search) ||
                 x.Client.FullName.Contains(filter.Search) ||
                 x.Property.Name.Contains(filter.Search)) &&
                (!filter.Status.HasValue || x.Status == filter.Status.Value) &&
                (!filter.PaymentStatus.HasValue || x.PaymentStatus == filter.PaymentStatus.Value) &&
                (!filter.PropertyId.HasValue || x.PropertyId == filter.PropertyId.Value) &&
                (string.IsNullOrWhiteSpace(filter.ClientEmail) || x.Client.Email.Contains(filter.ClientEmail)) &&
                (!filter.FromDate.HasValue || x.CreatedAt >= filter.FromDate.Value.Date) &&
                (!filter.ToDate.HasValue || x.CreatedAt < filter.ToDate.Value.Date.AddDays(1)))
        {
            AddInclude(x => x.Client);
            AddInclude(x => x.Property);
            ApplyOrderByDescending(x => x.CreatedAt);
            ApplyPaging((filter.PageIndex - 1) * filter.PageSize, filter.PageSize);
        }

        public BookingWithDetailsSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Client);
            AddInclude(x => x.Property);
            AddInclude("Property.Images");
        }
    }
}
