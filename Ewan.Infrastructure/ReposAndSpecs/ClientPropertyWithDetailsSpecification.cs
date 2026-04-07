using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Property;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class ClientPropertyWithDetailsSpecification : BaseSpecification<Property>
    {
        public ClientPropertyWithDetailsSpecification(ClientPropertyFilterParams filter)
            : base(x =>
                (string.IsNullOrWhiteSpace(filter.Search) ||
                 x.Name.Contains(filter.Search) ||
                 x.Address.Contains(filter.Search) ||
                 x.Location.Contains(filter.Search)) &&
                (!filter.PropertyType.HasValue || x.PropertyType == filter.PropertyType.Value) &&
                (!filter.MinAverageRate.HasValue ||
                 (x.Ratings.Any() && x.Ratings.Average(r => (double)r.Rate) >= filter.MinAverageRate.Value)))
        {
            AddInclude(x => x.Images);
            AddInclude("PropertyFacilities.Facility");
            AddInclude(x => x.Ratings);
            AddInclude(x => x.FavoritedByClients);

            if (filter.PriceSort == PriceSortOption.LowToHigh)
                ApplyOrderBy(x => x.PricePerNight);
            else if (filter.PriceSort == PriceSortOption.HighToLow)
                ApplyOrderByDescending(x => x.PricePerNight);
            else
                ApplyOrderByDescending(x => x.Id);

            ApplyPaging(
                (filter.PageIndex - 1) * filter.PageSize,
                filter.PageSize);
        }
    }
}
