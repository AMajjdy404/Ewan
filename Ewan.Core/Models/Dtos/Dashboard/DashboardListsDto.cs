namespace Ewan.Core.Models.Dtos.Dashboard
{
    public class DashboardListsDto
    {
        public List<DashboardTopPropertyDto> TopBookedProperties { get; set; } = new();
        public List<DashboardRecentBookingDto> RecentBookings { get; set; } = new();
    }
}
