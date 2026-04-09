using MediatR;

namespace Ewan.Application.Features.Dashboard.Clients.Commands.UpdateClientActivation
{
    public record UpdateClientActivationCommand(int ClientId, bool IsActive) : IRequest;
}
