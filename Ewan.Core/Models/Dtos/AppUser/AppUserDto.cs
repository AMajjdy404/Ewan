namespace Ewan.Core.Models.Dtos.AppUser
{
    public class AppUserDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
