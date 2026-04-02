using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Property;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class ClientPropertyCountSpecification : BaseSpecification<Property>
    {
        public ClientPropertyCountSpecification(ClientPropertyFilterParams filter)
            : base(x =>
                (string.IsNullOrWhiteSpace(filter.Search) ||
                 x.Name.Contains(filter.Search) ||
                 x.Address.Contains(filter.Search) ||
                 x.Location.Contains(filter.Search)) &&
                (!filter.GroupId.HasValue || x.GroupId == filter.GroupId.Value) &&
                (!filter.MinAverageRate.HasValue ||
                 (x.Ratings.Any() && x.Ratings.Average(r => (double)r.Rate) >= filter.MinAverageRate.Value)))
        {
        }
    }
}
