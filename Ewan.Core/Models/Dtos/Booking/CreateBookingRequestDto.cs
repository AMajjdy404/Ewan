using Ewan.Core.Models.Enums;

namespace Ewan.Core.Models.Dtos.Booking
{
    public class CreateBookingRequestDto
    {
        public int PropertyId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int RoomsCount { get; set; }
        public int GuestsCount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
