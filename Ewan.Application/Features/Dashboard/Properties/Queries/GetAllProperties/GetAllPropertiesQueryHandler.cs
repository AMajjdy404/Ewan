using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Property;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Ewan.Application.Features.Dashboard.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, Pagination<PropertyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _baseApiUrl;

        public GetAllPropertiesQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _baseApiUrl = configuration["BaseApiUrl"] ?? string.Empty;
        }

        public async Task<Pagination<PropertyDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var spec = new PropertyWithDetailsSpecification(request.Params);
            var countSpec = new PropertyCountSpecification();

            var properties = await _unitOfWork.Repository<Property>().ListAsync(spec);
            var totalCount = await _unitOfWork.Repository<Property>().CountAsync(countSpec);

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

            return new Pagination<PropertyDto>(
                request.Params.PageIndex,
                request.Params.PageSize,
                totalCount,
                data
            );
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
