using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Booking;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class ClientBookingWithDetailsSpecification : BaseSpecification<Booking>
    {
        public ClientBookingWithDetailsSpecification(int clientId, ClientBookingFilterParams pagination)
            : base(x => x.ClientId == clientId)
        {
            AddInclude(x => x.Property);
            AddInclude("Property.Images");
            AddInclude("Property.Ratings");
            AddInclude("Property.FavoritedByClients");

            ApplyOrderByDescending(x => x.CreatedAt);
            ApplyPaging((pagination.PageIndex - 1) * pagination.PageSize, pagination.PageSize);
        }
    }
}
