using Ewan.Core.Models.Dtos;

namespace Ewan.Core.Models.Dtos.Property
{
    public class DashboardPropertiesResultDto
    {
        public Pagination<PropertyDto> Properties { get; set; } = null!;
        public PropertyTypeCountsDto TypeCounts { get; set; } = new();
    }
}
