using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Property;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class PropertySummarySpecification : BaseSpecification<Property>
    {
        public PropertySummarySpecification(DashboardPropertyFilterParams filter)
            : base(p =>
                (string.IsNullOrWhiteSpace(filter.Search) || p.Name.Contains(filter.Search)) &&
                (!filter.PropertyType.HasValue || p.PropertyType == filter.PropertyType.Value))
        {
        }
    }
}
