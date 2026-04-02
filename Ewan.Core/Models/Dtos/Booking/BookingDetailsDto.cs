using Ewan.Core.Models.Enums;

namespace Ewan.Core.Models.Dtos.Booking
{
    public class BookingDetailsDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = null!;
        public string ClientEmail { get; set; } = null!;
        public string? ClientPhoneNumber { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; } = null!;
        public string PropertyAddress { get; set; } = null!;
        public string PropertyLocation { get; set; } = null!;
        public int PropertyRoomCount { get; set; }
        public int PropertyGuestCount { get; set; }
        public List<string> PropertyImageUrls { get; set; } = new();
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NightsCount { get; set; }
        public int RoomsCount { get; set; }
        public int GuestsCount { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }
    }
}
