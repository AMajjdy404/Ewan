using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class PropertyBookingsWithDetailsSpecification : BaseSpecification<Booking>
    {
        public PropertyBookingsWithDetailsSpecification(int propertyId, PaginationParams pagination)
            : base(x => x.PropertyId == propertyId)
        {
            AddInclude(x => x.Client);
            AddInclude(x => x.Property);
            AddInclude("Property.Images");
            ApplyOrderByDescending(x => x.CreatedAt);
            ApplyPaging((pagination.PageIndex - 1) * pagination.PageSize, pagination.PageSize);
        }
    }
}
