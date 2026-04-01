using Microsoft.AspNetCore.Http;

namespace Ewan.Core.Models.Dtos.Property
{
    public class CreatePropertyRequestDto
    {
        public string Name { get; set; } = null!;
        public int GroupId { get; set; }
        public string Address { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public int RoomCount { get; set; }
        public int GuestCount { get; set; }
        public List<IFormFile> Images { get; set; } = new();
        public List<int> FacilityIds { get; set; } = new();
    }
}
