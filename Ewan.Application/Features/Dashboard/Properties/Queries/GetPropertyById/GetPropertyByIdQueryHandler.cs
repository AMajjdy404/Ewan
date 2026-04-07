using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Property;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Ewan.Application.Features.Dashboard.Properties.Queries.GetPropertyById
{
    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _baseApiUrl;

        public GetPropertyByIdQueryHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _baseApiUrl = configuration["BaseApiUrl"] ?? string.Empty;
        }

        public async Task<PropertyDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new PropertyWithDetailsSpecification(request.Id);
            var property = await _unitOfWork.Repository<Property>().GetEntityWithSpec(spec);

            if (property == null)
                throw new KeyNotFoundException("Property not found.");

            return new PropertyDto
            {
                Id = property.Id,
                Name = property.Name,
                Description = property.Description,
                OwnerPhoneNumber = property.OwnerPhoneNumber,
                PropertyType = property.PropertyType,
                BookingMode = property.BookingMode,
                IsAvailable = property.IsAvailable,
                Address = property.Address,
                Location = property.Location,
                PricePerNight = property.PricePerNight,
                PricePerHour = property.PricePerHour,
                RoomCount = property.RoomCount,
                GuestCount = property.GuestCount,
                ImageUrls = property.Images.Select(i => ToAbsoluteImageUrl(i.ImageUrl)).ToList(),
                Facilities = property.PropertyFacilities.Select(pf => pf.Facility.Name).ToList()
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
