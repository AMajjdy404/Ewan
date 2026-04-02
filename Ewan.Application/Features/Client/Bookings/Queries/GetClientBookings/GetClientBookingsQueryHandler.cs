using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Booking;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;

namespace Ewan.Application.Features.Client.Bookings.Queries.GetClientBookings
{
    public class GetClientBookingsQueryHandler : IRequestHandler<GetClientBookingsQuery, Pagination<ClientBookingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetClientBookingsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Pagination<ClientBookingDto>> Handle(GetClientBookingsQuery request, CancellationToken cancellationToken)
        {
            var spec = new ClientBookingWithDetailsSpecification(request.ClientId, request.Params);
            var countSpec = new ClientBookingCountSpecification(request.ClientId);

            var bookings = await _unitOfWork.Repository<Booking>().ListAsync(spec);
            var totalCount = await _unitOfWork.Repository<Booking>().CountAsync(countSpec);

            var data = bookings.Select(x => new ClientBookingDto
            {
                Id = x.Id,
                PropertyId = x.PropertyId,
                PropertyName = x.Property.Name,
                PropertyMainImage = x.Property.Images.OrderBy(i => i.Id).Select(i => i.ImageUrl).FirstOrDefault(),
                CheckInDate = x.CheckInDate,
                CheckOutDate = x.CheckOutDate,
                NightsCount = x.NightsCount,
                RoomsCount = x.RoomsCount,
                GuestsCount = x.GuestsCount,
                TotalAmount = x.TotalAmount,
                Status = x.Status,
                PaymentStatus = x.PaymentStatus,
                PaymentMethod = x.PaymentMethod,
                AverageRate = x.Property.Ratings.Any() ? Math.Round(x.Property.Ratings.Average(r => r.Rate), 1) : 0,
                IsFavourite = x.Property.FavoritedByClients.Any(f => f.ClientId == request.ClientId),
                CreatedAt = x.CreatedAt
            }).ToList();

            return new Pagination<ClientBookingDto>(
                request.Params.PageIndex,
                request.Params.PageSize,
                totalCount,
                data);
        }
    }
}
