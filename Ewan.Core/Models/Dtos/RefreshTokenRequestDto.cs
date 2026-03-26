namespace Ewan.Core.Models.Dtos
{
    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string? DeviceInfo { get; set; }
        public string? DeviceId { get; set; }
    }
}
