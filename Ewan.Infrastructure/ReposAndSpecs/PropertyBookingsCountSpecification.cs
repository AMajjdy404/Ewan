using Ewan.Core.Models;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class PropertyBookingsCountSpecification : BaseSpecification<Booking>
    {
        public PropertyBookingsCountSpecification(int propertyId)
            : base(x => x.PropertyId == propertyId)
        {
        }
    }
}
