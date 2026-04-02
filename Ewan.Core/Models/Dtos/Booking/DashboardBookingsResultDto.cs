using Ewan.Core.Models.Dtos;

namespace Ewan.Core.Models.Dtos.Booking
{
    public class DashboardBookingsResultDto
    {
        public Pagination<BookingDto> Bookings { get; set; } = null!;
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public BookingStatusTotalsDto StatusTotals { get; set; } = new();
    }
}
