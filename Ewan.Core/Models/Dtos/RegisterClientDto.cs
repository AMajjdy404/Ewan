namespace Ewan.Core.Models.Dtos
{
    public class RegisterClientDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
        public string? DeviceInfo { get; set; }
        public string? DeviceId { get; set; }
    }
}
