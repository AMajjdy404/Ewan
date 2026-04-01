using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class FacilityWithPaginationSpecification : BaseSpecification<Facility>
    {
        public FacilityWithPaginationSpecification(PaginationParams paginationParams)
        {
            ApplyPaging(
                (paginationParams.PageIndex - 1) * paginationParams.PageSize,
                paginationParams.PageSize);

            ApplyOrderBy(x => x.Name);
        }
    }
}
