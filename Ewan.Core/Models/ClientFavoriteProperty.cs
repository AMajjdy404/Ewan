namespace Ewan.Core.Models
{
    public class ClientFavoriteProperty
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PropertyId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Client Client { get; set; } = null!;
        public Property Property { get; set; } = null!;
    }
}
