using MediatR;

namespace Ewan.Application.Features.Client.Properties.Commands.AddFavoriteProperty
{
    public record AddFavoritePropertyCommand(int ClientId, int PropertyId) : IRequest;
}
