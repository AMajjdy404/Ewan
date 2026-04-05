using MediatR;

namespace Ewan.Application.Features.Client.Profile.Commands.DeleteClientAccount
{
    public record DeleteClientAccountCommand(int ClientId) : IRequest;
}
