
 using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Facility;
 using MediatR;

namespace Ewan.Application.Features.Dashboard.Facilities.Queries.GetFacilityById
{

    public class GetFacilityByIdQueryHandler : IRequestHandler<GetFacilityByIdQuery, FacilityDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFacilityByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FacilityDto> Handle(GetFacilityByIdQuery request, CancellationToken cancellationToken)
        {
            var facility = await _unitOfWork.Repository<Facility>().GetByIdAsync(request.Id);

            if (facility == null)
                throw new KeyNotFoundException("Facility not found.");

            return new FacilityDto
            {
                Id = facility.Id,
                Name = facility.Name
            };
        }
    }
}
