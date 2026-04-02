using Ewan.Core.Models.Enums;

namespace Ewan.Core.Models.Dtos.Booking
{
    public class ClientBookingDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; } = null!;
        public string? PropertyMainImage { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NightsCount { get; set; }
        public int RoomsCount { get; set; }
        public int GuestsCount { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public double AverageRate { get; set; }
        public bool IsFavourite { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
