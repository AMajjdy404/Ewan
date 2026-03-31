using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.PropertyGroup;
using Ewan.Core.Specifications;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
namespace Ewan.Application.Features.Dashboard.PropertyGroups.Queries.GetAllPropertyGroups
{

    public class GetAllPropertyGroupsQueryHandler
    : IRequestHandler<GetAllPropertyGroupsQuery, Pagination<PropertyGroupDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPropertyGroupsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Pagination<PropertyGroupDto>> Handle(
            GetAllPropertyGroupsQuery request,
            CancellationToken cancellationToken)
        {
            var spec = new PropertyGroupWithPaginationSpecification(request.Params);

            var repo = _unitOfWork.Repository<PropertyGroup>();

            var totalCount = await repo.CountAsync(new PropertyGroupCountSpecification());

            var data = await repo.ListAsync(spec);

            var mapped = data.Select(x => new PropertyGroupDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return new Pagination<PropertyGroupDto>(
                request.Params.PageIndex,
                request.Params.PageSize,
                totalCount,
                mapped
            );
        }
    }
}
