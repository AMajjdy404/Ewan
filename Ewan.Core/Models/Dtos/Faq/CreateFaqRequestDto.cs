namespace Ewan.Core.Models.Dtos.Faq
{
    public class CreateFaqRequestDto
    {
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
    }
}
