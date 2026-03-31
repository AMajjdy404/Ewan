using Ewan.Core.Services;
using MediatR;

namespace Ewan.Application.Features.Auth.Commands.LogoutAllDevices
{
    public class LogoutAllDevicesCommandHandler : IRequestHandler<LogoutAllDevicesCommand, Unit>
    {
        private readonly ITokenService _tokenService;

        public LogoutAllDevicesCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<Unit> Handle(LogoutAllDevicesCommand command, CancellationToken cancellationToken)
        {
            await _tokenService.RevokeAllUserRefreshTokensAsync(
                command.OwnerId,
                command.UserType,
                command.IpAddress);

            return Unit.Value;
        }
    }
}