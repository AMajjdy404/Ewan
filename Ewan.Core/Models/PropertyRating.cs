namespace Ewan.Core.Models
{
    public class PropertyRating
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PropertyId { get; set; }
        public int Rate { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Client Client { get; set; } = null!;
        public Property Property { get; set; } = null!;
    }
}
