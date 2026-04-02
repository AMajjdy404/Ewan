using Ewan.Core.Models.Enums;

namespace Ewan.Core.Models.Dtos.Booking
{
    public class BookingFilterParams : PaginationParams
    {
        public string? Search { get; set; }
        public BookingStatus? Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public int? PropertyId { get; set; }
        public string? ClientEmail { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
