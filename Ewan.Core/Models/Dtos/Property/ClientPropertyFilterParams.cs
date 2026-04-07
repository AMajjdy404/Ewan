using Ewan.Core.Models.Enums;

namespace Ewan.Core.Models.Dtos.Property
{
    public class ClientPropertyFilterParams : PaginationParams
    {
        public string? Search { get; set; }
        public PropertyType? PropertyType { get; set; }
        public PriceSortOption PriceSort { get; set; } = PriceSortOption.None;
        public double? MinAverageRate { get; set; }
    }
}
