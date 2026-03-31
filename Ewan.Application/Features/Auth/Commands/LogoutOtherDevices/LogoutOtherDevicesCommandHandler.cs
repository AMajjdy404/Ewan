using Ewan.Core.Services;
using MediatR;

namespace Ewan.Application.Features.Auth.Commands.LogoutOtherDevices
{
    public class LogoutOtherDevicesCommandHandler : IRequestHandler<LogoutOtherDevicesCommand, Unit>
    {
        private readonly ITokenService _tokenService;

        public LogoutOtherDevicesCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<Unit> Handle(LogoutOtherDevicesCommand command, CancellationToken cancellationToken)
        {
            await _tokenService.RevokeOtherDeviceTokensAsync(
                command.OwnerId,
                command.UserType,
                command.CurrentDeviceId,
                command.IpAddress);

            return Unit.Value;
        }
    }
}
