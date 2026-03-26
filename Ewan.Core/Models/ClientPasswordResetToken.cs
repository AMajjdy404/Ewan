namespace Ewan.Core.Models
{
    public class ClientPasswordResetToken
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string TokenHash { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UsedAt { get; set; }

        public string? CreatedByIpAddress { get; set; }
    }
}
