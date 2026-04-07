using Ewan.Core.Models;
using Ewan.Core.Models.Enums;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class ActiveBookingsForPropertiesSpecification : BaseSpecification<Booking>
    {
        public ActiveBookingsForPropertiesSpecification(IReadOnlyCollection<int> propertyIds, DateTime atUtc)
            : base(x => propertyIds.Contains(x.PropertyId)
                        && x.Status != BookingStatus.Cancelled
                        && x.CheckInDate <= atUtc
                        && x.CheckOutDate > atUtc)
        {
        }
    }
}
