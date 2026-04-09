using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Property;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class PropertyWithDetailsSpecification : BaseSpecification<Property>
    {
        // GetAll with pagination
        public PropertyWithDetailsSpecification(DashboardPropertyFilterParams filter)
            : base(p =>
                (string.IsNullOrWhiteSpace(filter.Search) || p.Name.Contains(filter.Search)) &&
                (!filter.PropertyType.HasValue || p.PropertyType == filter.PropertyType.Value))
        {
            AddInclude(p => p.Images);
            AddInclude("PropertyFacilities.Facility");
            ApplyOrderBy(p => p.Id);
            ApplyPaging(
                (filter.PageIndex - 1) * filter.PageSize,
                filter.PageSize
            );
        }

        // GetById
        public PropertyWithDetailsSpecification(int id)
            : base(p => p.Id == id)
        {
            AddInclude(p => p.Images);
            AddInclude("PropertyFacilities.Facility");
        }
    }
}
