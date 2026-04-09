using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class ClientFavouritePropertiesSpecification : BaseSpecification<Property>
    {
        public ClientFavouritePropertiesSpecification(int clientId, PaginationParams pagination)
            : base(x => x.FavoritedByClients.Any(f => f.ClientId == clientId))
        {
            AddInclude(x => x.Images);
            AddInclude("PropertyFacilities.Facility");
            AddInclude(x => x.Ratings);
            AddInclude(x => x.FavoritedByClients);
            ApplyOrderByDescending(x => x.Id);
            ApplyPaging((pagination.PageIndex - 1) * pagination.PageSize, pagination.PageSize);
        }
    }
}
