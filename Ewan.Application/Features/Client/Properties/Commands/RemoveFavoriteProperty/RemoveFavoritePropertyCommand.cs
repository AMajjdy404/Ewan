using MediatR;

namespace Ewan.Application.Features.Client.Properties.Commands.RemoveFavoriteProperty
{
    public record RemoveFavoritePropertyCommand(int ClientId, int PropertyId) : IRequest;
}
