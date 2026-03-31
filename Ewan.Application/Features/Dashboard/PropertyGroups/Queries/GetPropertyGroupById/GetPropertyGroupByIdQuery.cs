using Ewan.Core.Models.Dtos.PropertyGroup;
using MediatR;

namespace Ewan.Application.Features.Dashboard.PropertyGroups.Queries.GetPropertyGroupById
{
    public record GetPropertyGroupByIdQuery(int Id) : IRequest<PropertyGroupDto>;
}
