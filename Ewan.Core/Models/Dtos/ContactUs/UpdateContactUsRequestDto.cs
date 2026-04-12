namespace Ewan.Core.Models.Dtos.ContactUs
{
    public class UpdateContactUsRequestDto
    {
        public string SupportNumber { get; set; } = null!;
        public string WhatsappNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
