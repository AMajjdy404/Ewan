using Ewan.Core.Models.Enums;

namespace Ewan.Core.Models.Dtos.Property
{
    public class ClientPropertyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public PropertyType PropertyType { get; set; }
        public BookingMode BookingMode { get; set; }
        public bool IsAvailable { get; set; }
        public string Address { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public decimal PricePerHour { get; set; }
        public int RoomCount { get; set; }
        public int AvailableRoomCount { get; set; }
        public int GuestCount { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public List<string> Facilities { get; set; } = new();
        public double AverageRate { get; set; }
        public bool IsFavourite { get; set; }
    }
}
