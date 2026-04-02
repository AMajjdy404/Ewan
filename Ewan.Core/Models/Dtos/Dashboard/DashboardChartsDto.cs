namespace Ewan.Core.Models.Dtos.Dashboard
{
    public class DashboardChartsDto
    {
        public List<DashboardMonthlyPointDto> RevenueTrend { get; set; } = new();
        public List<DashboardMonthlyPointDto> BookingsTrend { get; set; } = new();
        public List<DashboardDistributionItemDto> BookingsByCategory { get; set; } = new();
        public List<DashboardDistributionItemDto> BookingsByCity { get; set; } = new();
    }
}
