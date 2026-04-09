using Microsoft.AspNetCore.Http;
using Ewan.Core.Models.Enums;

namespace Ewan.Core.Models.Dtos.Property
{
    public class UpdatePropertyRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public PropertyType PropertyType { get; set; }
        public bool IsAvailable { get; set; }
        public string Address { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal? PricePerNight { get; set; }
        public decimal? PricePerHour { get; set; }
        public int? RoomCount { get; set; }
        public int GuestCount { get; set; }
        public List<IFormFile> NewImages { get; set; } = new();
        public List<string> ExistingImageUrls { get; set; } = new();
        public List<int> FacilityIds { get; set; } = new();
    }
}
