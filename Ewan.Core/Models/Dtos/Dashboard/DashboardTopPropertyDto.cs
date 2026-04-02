namespace Ewan.Core.Models.Dtos.Dashboard
{
    public class DashboardTopPropertyDto
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; } = null!;
        public int BookingsCount { get; set; }
        public decimal Revenue { get; set; }
    }
}
