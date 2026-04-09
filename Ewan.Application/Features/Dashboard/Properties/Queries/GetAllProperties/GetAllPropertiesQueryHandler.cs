using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Enums;
using Ewan.Core.Models.Dtos.Property;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Ewan.Application.Features.Dashboard.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, DashboardPropertiesResultDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _baseApiUrl;

        public GetAllPropertiesQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _baseApiUrl = configuration["BaseApiUrl"] ?? string.Empty;
        }

        public async Task<DashboardPropertiesResultDto> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var spec = new PropertyWithDetailsSpecification(request.Params);
            var countSpec = new PropertyCountSpecification(request.Params);
            var summarySpec = new PropertySummarySpecification(request.Params);

            var repo = _unitOfWork.Repository<Property>();
            var properties = await repo.ListAsync(spec);
            var totalCount = await repo.CountAsync(countSpec);
            var filteredProperties = await repo.ListAsync(summarySpec);

            var data = properties.Select(p => new PropertyDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                OwnerPhoneNumber = p.OwnerPhoneNumber,
                PropertyType = p.PropertyType,
                BookingMode = p.BookingMode,
                IsAvailable = p.IsAvailable,
                Address = p.Address,
                Location = p.Location,
                PricePerNight = p.PricePerNight,
                PricePerHour = p.PricePerHour,
                RoomCount = p.RoomCount,
                GuestCount = p.GuestCount,
                ImageUrls = p.Images.Select(i => ToAbsoluteImageUrl(i.ImageUrl)).ToList(),
                Facilities = p.PropertyFacilities.Select(pf => pf.Facility.Name).ToList()
            }).ToList();

            var pagination = new Pagination<PropertyDto>(
                request.Params.PageIndex,
                request.Params.PageSize,
                totalCount,
                data
            );

            var typeCounts = new PropertyTypeCountsDto
            {
                ChaletCount = filteredProperties.Count(x => x.PropertyType == PropertyType.Chalet),
                HotelCount = filteredProperties.Count(x => x.PropertyType == PropertyType.Hotel),
                ApartmentCount = filteredProperties.Count(x => x.PropertyType == PropertyType.Apartment),
                HallCount = filteredProperties.Count(x => x.PropertyType == PropertyType.Hall),
                TotalCount = filteredProperties.Count
            };

            return new DashboardPropertiesResultDto
            {
                Properties = pagination,
                TypeCounts = typeCounts
            };
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
    }
}
