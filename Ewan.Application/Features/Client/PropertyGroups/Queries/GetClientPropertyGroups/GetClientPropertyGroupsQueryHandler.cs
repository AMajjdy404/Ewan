using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.PropertyGroup;
using MediatR;

namespace Ewan.Application.Features.Client.PropertyGroups.Queries.GetClientPropertyGroups
{
    public class GetClientPropertyGroupsQueryHandler : IRequestHandler<GetClientPropertyGroupsQuery, IReadOnlyList<PropertyGroupDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetClientPropertyGroupsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<PropertyGroupDto>> Handle(GetClientPropertyGroupsQuery request, CancellationToken cancellationToken)
        {
            var groups = await _unitOfWork.Repository<PropertyGroup>().ListAllAsync();

            return groups
                .OrderBy(x => x.Name)
                .Select(x => new PropertyGroupDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();
        }
    }
}
