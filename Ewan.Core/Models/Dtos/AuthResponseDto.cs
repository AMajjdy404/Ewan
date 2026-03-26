namespace Ewan.Core.Models.Dtos
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
    }
}
