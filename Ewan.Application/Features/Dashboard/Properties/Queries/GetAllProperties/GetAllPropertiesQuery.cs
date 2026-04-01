using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Property;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Properties.Queries.GetAllProperties
{
    public record GetAllPropertiesQuery(PaginationParams Params) : IRequest<Pagination<PropertyDto>>;


}
