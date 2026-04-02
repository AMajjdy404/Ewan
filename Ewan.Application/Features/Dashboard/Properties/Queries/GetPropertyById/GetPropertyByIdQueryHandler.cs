using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Property;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Properties.Queries.GetPropertyById
{
    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetPropertyByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                GroupId = property.GroupId,
                GroupName = property.Group.Name,
                IsAvailable = property.IsAvailable,
                Address = property.Address,
                Location = property.Location,
                PricePerNight = property.PricePerNight,
                RoomCount = property.RoomCount,
                GuestCount = property.GuestCount,
                ImageUrls = property.Images.Select(i => i.ImageUrl).ToList(),
                Facilities = property.PropertyFacilities.Select(pf => pf.Facility.Name).ToList()
            };
        }
    }
}
