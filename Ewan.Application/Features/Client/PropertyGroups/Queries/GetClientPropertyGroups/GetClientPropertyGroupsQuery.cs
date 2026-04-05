using Ewan.Core.Models.Dtos.PropertyGroup;
using MediatR;

namespace Ewan.Application.Features.Client.PropertyGroups.Queries.GetClientPropertyGroups
{
    public record GetClientPropertyGroupsQuery : IRequest<IReadOnlyList<PropertyGroupDto>>;
}
