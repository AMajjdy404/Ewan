namespace Ewan.Core.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int GroupId { get; set; }
        public bool IsAvailable { get; set; }
        public string Address { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public int RoomCount { get; set; }
        public int GuestCount { get; set; }

        public PropertyGroup Group { get; set; } = null!;
        public ICollection<PropertyImage> Images { get; set; } = new HashSet<PropertyImage>();
        public ICollection<PropertyFacility> PropertyFacilities { get; set; } = new HashSet<PropertyFacility>();
    }
}
