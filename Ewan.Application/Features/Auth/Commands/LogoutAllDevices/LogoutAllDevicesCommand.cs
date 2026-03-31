using MediatR;

namespace Ewan.Application.Features.Auth.Commands.LogoutAllDevices
{
    public record LogoutAllDevicesCommand(
        string OwnerId,
        string UserType,
        string IpAddress
    ) : IRequest<Unit>;
}