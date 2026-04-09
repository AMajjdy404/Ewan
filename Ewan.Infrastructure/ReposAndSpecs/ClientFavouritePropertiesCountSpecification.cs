using Ewan.Core.Models;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class ClientFavouritePropertiesCountSpecification : BaseSpecification<Property>
    {
        public ClientFavouritePropertiesCountSpecification(int clientId)
            : base(x => x.FavoritedByClients.Any(f => f.ClientId == clientId))
        {
        }
    }
}
