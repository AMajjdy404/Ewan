using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class PropertyWithDetailsSpecification : BaseSpecification<Property>
    {
        // GetAll with pagination
        public PropertyWithDetailsSpecification(PaginationParams paginationParams)
        {
            AddInclude(p => p.Images);
            AddInclude("PropertyFacilities.Facility");
            ApplyOrderBy(p => p.Id);
            ApplyPaging(
                (paginationParams.PageIndex - 1) * paginationParams.PageSize,
                paginationParams.PageSize
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
