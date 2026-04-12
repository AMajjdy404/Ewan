namespace Ewan.Core.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DeviceToken { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public ICollection<ClientFavoriteProperty> FavoriteProperties { get; set; } = new HashSet<ClientFavoriteProperty>();
        public ICollection<PropertyRating> Ratings { get; set; } = new HashSet<PropertyRating>();
    }
}
