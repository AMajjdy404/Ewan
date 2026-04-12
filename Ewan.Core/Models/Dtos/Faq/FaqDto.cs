namespace Ewan.Core.Models.Dtos.Faq
{
    public class FaqDto
    {
        public int Id { get; set; }
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
    }
}
