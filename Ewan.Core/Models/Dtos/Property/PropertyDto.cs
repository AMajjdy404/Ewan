namespace Ewan.Core.Models.Dtos.Property
{
    public class PropertyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public int GroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public string Address { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public int RoomCount { get; set; }
        public int GuestCount { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public List<string> Facilities { get; set; } = new();
    }
}
