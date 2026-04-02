namespace Ewan.Core.Models.Dtos.Property
{
    public class PropertyRatingItemDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; } = null!;
        public int Rate { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
