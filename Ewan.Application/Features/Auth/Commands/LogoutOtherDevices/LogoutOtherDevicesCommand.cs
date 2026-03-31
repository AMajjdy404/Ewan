using MediatR;

namespace Ewan.Application.Features.Auth.Commands.LogoutOtherDevices
{
    public record LogoutOtherDevicesCommand(
        string OwnerId,
        string UserType,
        string CurrentDeviceId,
        string IpAddress
    ) : IRequest<Unit>;
}