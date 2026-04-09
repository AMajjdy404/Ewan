namespace Ewan.Core.Models.Dtos.Property
{
    public class UpdatePropertyOwnerCredentialsRequestDto
    {
        public string OwnerPhoneNumber { get; set; } = null!;
        public string? OwnerPassword { get; set; }
    }
}
