using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Booking;
using Ewan.Core.Models.Enums;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Bookings.Queries.GetAllBookings
{
    public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, DashboardBookingsResultDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBookingsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardBookingsResultDto> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            var spec = new BookingWithDetailsSpecification(request.Params);
            var countSpec = new BookingCountSpecification(request.Params);
            var summarySpec = new BookingSummarySpecification(request.Params);

            var bookingRepo = _unitOfWork.Repository<Booking>();
            var bookings = await bookingRepo.ListAsync(spec);
            var totalCount = await bookingRepo.CountAsync(countSpec);
            var filteredBookings = await bookingRepo.ListAsync(summarySpec);

            var data = bookings.Select(x => new BookingDto
            {
                Id = x.Id,
                ClientName = x.Client.FullName,
                PropertyName = x.Property.Name,
                CheckInDate = x.CheckInDate,
                CheckOutDate = x.CheckOutDate,
                NightsCount = x.NightsCount,
                RoomsCount = x.RoomsCount,
                GuestsCount = x.GuestsCount,
                TotalAmount = x.TotalAmount,
                Status = x.Status,
                PaymentStatus = x.PaymentStatus,
                PaymentMethod = x.PaymentMethod,
                CreatedAt = x.CreatedAt
            }).ToList();

            var pagination = new Pagination<BookingDto>(
                request.Params.PageIndex,
                request.Params.PageSize,
                totalCount,
                data);

            return new DashboardBookingsResultDto
            {
                Bookings = pagination,
                TotalBookings = filteredBookings.Count,
                TotalRevenue = filteredBookings.Sum(x => x.TotalAmount),
                StatusTotals = new BookingStatusTotalsDto
                {
                    Pending = filteredBookings.Count(x => x.Status == BookingStatus.Pending),
                    Confirmed = filteredBookings.Count(x => x.Status == BookingStatus.Confirmed),
                    Completed = filteredBookings.Count(x => x.Status == BookingStatus.Completed),
                    Cancelled = filteredBookings.Count(x => x.Status == BookingStatus.Cancelled)
                }
            };
        }
    }
}
