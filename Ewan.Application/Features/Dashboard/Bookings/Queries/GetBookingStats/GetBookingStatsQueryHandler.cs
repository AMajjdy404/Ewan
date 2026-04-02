using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Booking;
using Ewan.Core.Models.Enums;
using Ewan.Core.Specifications;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Bookings.Queries.GetBookingStats
{
    public class GetBookingStatsQueryHandler : IRequestHandler<GetBookingStatsQuery, BookingStatsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBookingStatsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BookingStatsDto> Handle(GetBookingStatsQuery request, CancellationToken cancellationToken)
        {
            var bookingRepo = _unitOfWork.Repository<Booking>();

            var total = await bookingRepo.CountAsync(new BookingStatusSpecification());
            var confirmed = await bookingRepo.CountAsync(new BookingStatusSpecification(BookingStatus.Confirmed));
            var completed = await bookingRepo.CountAsync(new BookingStatusSpecification(BookingStatus.Completed));
            var cancelled = await bookingRepo.CountAsync(new BookingStatusSpecification(BookingStatus.Cancelled));
            var pending = await bookingRepo.CountAsync(new BookingStatusSpecification(BookingStatus.Pending));

            return new BookingStatsDto
            {
                TotalBookings = total,
                ConfirmedBookings = confirmed,
                CompletedBookings = completed,
                CancelledBookings = cancelled,
                PendingBookings = pending
            };
        }

        private class BookingStatusSpecification : BaseSpecification<Booking>
        {
            public BookingStatusSpecification()
            {
            }

            public BookingStatusSpecification(BookingStatus status)
                : base(x => x.Status == status)
            {
            }
        }
    }
}
