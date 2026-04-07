using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Enums;
using Ewan.Core.Models.Dtos.Property;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Ewan.Application.Features.Client.Properties.Queries.GetAllClientProperties
{
    public class GetAllClientPropertiesQueryHandler : IRequestHandler<GetAllClientPropertiesQuery, Pagination<ClientPropertyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _baseApiUrl;

        public GetAllClientPropertiesQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _baseApiUrl = configuration["BaseApiUrl"] ?? string.Empty;
        }

        public async Task<Pagination<ClientPropertyDto>> Handle(GetAllClientPropertiesQuery request, CancellationToken cancellationToken)
        {
            var spec = new ClientPropertyWithDetailsSpecification(request.Params);
            var countSpec = new ClientPropertyCountSpecification(request.Params);

            var properties = await _unitOfWork.Repository<Property>().ListAsync(spec);
            var totalCount = await _unitOfWork.Repository<Property>().CountAsync(countSpec);

            var propertyIds = properties.Select(x => x.Id).ToList();
            var activeBookings = propertyIds.Count == 0
                ? new List<Booking>()
                : (await _unitOfWork.Repository<Booking>()
                    .ListAsync(new ActiveBookingsForPropertiesSpecification(propertyIds, DateTime.UtcNow)))
                    .ToList();

            var activeBookingsByProperty = activeBookings
                .GroupBy(x => x.PropertyId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var data = properties.Select(p => new ClientPropertyDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                PropertyType = p.PropertyType,
                BookingMode = p.BookingMode,
                IsAvailable = p.IsAvailable,
                Address = p.Address,
                Location = p.Location,
                PricePerNight = p.PricePerNight,
                PricePerHour = p.PricePerHour,
                RoomCount = p.RoomCount,
                AvailableRoomCount = GetAvailableRoomsCount(p, activeBookingsByProperty),
                GuestCount = p.GuestCount,
                ImageUrls = p.Images.Select(i => ToAbsoluteImageUrl(i.ImageUrl)).ToList(),
                Facilities = p.PropertyFacilities.Select(pf => pf.Facility.Name).ToList(),
                AverageRate = p.Ratings.Any() ? Math.Round(p.Ratings.Average(r => r.Rate), 1) : 0,
                IsFavourite = p.FavoritedByClients.Any(f => f.ClientId == request.ClientId)
            }).ToList();

            return new Pagination<ClientPropertyDto>(
                request.Params.PageIndex,
                request.Params.PageSize,
                totalCount,
                data);
        }

        private string ToAbsoluteImageUrl(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return imageUrl;

            if (Uri.TryCreate(imageUrl, UriKind.Absolute, out _))
                return imageUrl;

            if (string.IsNullOrWhiteSpace(_baseApiUrl))
                return imageUrl;

            return $"{_baseApiUrl.TrimEnd('/')}/{imageUrl.TrimStart('/')}";
        }

        private static int GetAvailableRoomsCount(Property property, Dictionary<int, List<Booking>> activeBookingsByProperty)
        {
            activeBookingsByProperty.TryGetValue(property.Id, out var propertyActiveBookings);
            propertyActiveBookings ??= new List<Booking>();

            return property.BookingMode switch
            {
                BookingMode.RoomBased => Math.Max(0, property.RoomCount - propertyActiveBookings.Sum(x => x.RoomsCount)),
                BookingMode.ExclusiveStay => propertyActiveBookings.Count > 0 ? 0 : property.RoomCount,
                _ => 0
            };
        }
    }
}
