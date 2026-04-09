using Ewan.Core.Models.Enums;

namespace Ewan.Core.Models.Dtos.Property
{
    public class DashboardPropertyFilterParams : PaginationParams
    {
        public string? Search { get; set; }
        public PropertyType? PropertyType { get; set; }
    }
}
