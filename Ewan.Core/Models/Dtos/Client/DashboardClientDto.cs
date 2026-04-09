namespace Ewan.Core.Models.Dtos.Client
{
    public class DashboardClientDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public int BookingsCount { get; set; }
        public decimal TotalBookingsAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
