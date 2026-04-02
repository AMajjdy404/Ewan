namespace Ewan.Core.Models.Dtos.Property
{
    public class PropertyRatingsDto
    {
        public int PropertyId { get; set; }
        public double AverageRate { get; set; }
        public int RatingsCount { get; set; }
        public List<PropertyRatingItemDto> Ratings { get; set; } = new();
    }
}
