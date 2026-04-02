using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Property;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, Pagination<PropertyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllPropertiesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                GroupId = p.GroupId,
                GroupName = p.Group.Name,
                IsAvailable = p.IsAvailable,
                Address = p.Address,
                Location = p.Location,
                PricePerNight = p.PricePerNight,
                RoomCount = p.RoomCount,
                GuestCount = p.GuestCount,
                ImageUrls = p.Images.Select(i => i.ImageUrl).ToList(),
                Facilities = p.PropertyFacilities.Select(pf => pf.Facility.Name).ToList()
            }).ToList();

            return new Pagination<PropertyDto>(
                request.Params.PageIndex,
                request.Params.PageSize,
                totalCount,
                data
            );
        }
    }
}
