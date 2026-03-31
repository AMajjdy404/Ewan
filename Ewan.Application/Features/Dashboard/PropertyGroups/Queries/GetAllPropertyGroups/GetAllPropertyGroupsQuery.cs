using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.PropertyGroup;
using MediatR;

namespace Ewan.Application.Features.Dashboard.PropertyGroups.Queries.GetAllPropertyGroups
{
    public record GetAllPropertyGroupsQuery(PaginationParams Params)
    : IRequest<Pagination<PropertyGroupDto>>;
}
