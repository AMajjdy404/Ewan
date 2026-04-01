using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Facility;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Facilities.Queries.GetAllFacilities
{

    public class GetAllFacilitiesQueryHandler : IRequestHandler<GetAllFacilitiesQuery, Pagination<FacilityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllFacilitiesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Pagination<FacilityDto>> Handle(GetAllFacilitiesQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Facility>();

            var spec = new FacilityWithPaginationSpecification(request.Params);
            var countSpec = new FacilityCountSpecification();

            var totalCount = await repo.CountAsync(countSpec);
            var facilities = await repo.ListAsync(spec);

            var data = facilities.Select(x => new FacilityDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return new Pagination<FacilityDto>(
                request.Params.PageIndex,
                request.Params.PageSize,
                totalCount,
                data
            );
        }
    }
}
