using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Property;
using MediatR;

namespace Ewan.Application.Features.Client.Properties.Queries.GetFavouriteClientProperties
{
    public record GetFavouriteClientPropertiesQuery(int ClientId, PaginationParams Params)
        : IRequest<Pagination<ClientPropertyDto>>;
}
