using MediatR;

namespace Ewan.Application.Features.Auth.Commands.Logout
{
    public record LogoutCommand(
        string RefreshToken,
        string IpAddress
    ) : IRequest<Unit>;
}