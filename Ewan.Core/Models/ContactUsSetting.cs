namespace Ewan.Core.Models
{
    public class ContactUsSetting
    {
        public int Id { get; set; }
        public string SupportNumber { get; set; } = null!;
        public string WhatsappNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
