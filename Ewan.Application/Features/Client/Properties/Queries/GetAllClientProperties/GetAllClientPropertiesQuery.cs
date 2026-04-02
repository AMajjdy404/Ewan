using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Property;
using MediatR;

namespace Ewan.Application.Features.Client.Properties.Queries.GetAllClientProperties
{
    public record GetAllClientPropertiesQuery(int ClientId, ClientPropertyFilterParams Params)
        : IRequest<Pagination<ClientPropertyDto>>;
}
