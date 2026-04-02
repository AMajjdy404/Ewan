using Ewan.Core.Models.Enums;

namespace Ewan.Core.Models.Dtos.Dashboard
{
    public class DashboardRecentBookingDto
    {
        public int BookingId { get; set; }
        public string ClientName { get; set; } = null!;
        public string PropertyName { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
