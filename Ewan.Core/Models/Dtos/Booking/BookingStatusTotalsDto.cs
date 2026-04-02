namespace Ewan.Core.Models.Dtos.Booking
{
    public class BookingStatusTotalsDto
    {
        public int Pending { get; set; }
        public int Confirmed { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
    }
}
