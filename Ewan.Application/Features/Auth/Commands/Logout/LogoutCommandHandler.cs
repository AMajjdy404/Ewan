using Ewan.Core.Services;
using MediatR;

namespace Ewan.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly ITokenService _tokenService;

        public LogoutCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<Unit> Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(command.RefreshToken))
                await _tokenService.RevokeRefreshTokenAsync(command.RefreshToken, command.IpAddress);

            return Unit.Value;
        }
    }
}