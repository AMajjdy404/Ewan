using Ewan.Core.Models.Dtos;
using MediatR;

namespace Ewan.Application.Features.Auth.Commands.LoginClient
{
    public record LoginClientCommand(
        ClientLoginRequestDto Request,
        string IpAddress
    ) : IRequest<AuthResponseDto>;
}