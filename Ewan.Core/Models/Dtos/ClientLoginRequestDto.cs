namespace Ewan.Core.Models.Dtos
{
    public class ClientLoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
        public string? DeviceInfo { get; set; }
        public string? DeviceId { get; set; }
    }
}
