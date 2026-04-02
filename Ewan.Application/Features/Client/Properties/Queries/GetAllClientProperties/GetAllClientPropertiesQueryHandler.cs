using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Property;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;

namespace Ewan.Application.Features.Client.Properties.Queries.GetAllClientProperties
{
    public class GetAllClientPropertiesQueryHandler : IRequestHandler<GetAllClientPropertiesQuery, Pagination<ClientPropertyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllClientPropertiesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Pagination<ClientPropertyDto>> Handle(GetAllClientPropertiesQuery request, CancellationToken cancellationToken)
        {
            var spec = new ClientPropertyWithDetailsSpecification(request.Params);
            var countSpec = new ClientPropertyCountSpecification(request.Params);

            var properties = await _unitOfWork.Repository<Property>().ListAsync(spec);
            var totalCount = await _unitOfWork.Repository<Property>().CountAsync(countSpec);

            var data = properties.Select(p => new ClientPropertyDto
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
    }
}
