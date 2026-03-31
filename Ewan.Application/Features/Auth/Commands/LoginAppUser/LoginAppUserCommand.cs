using Ewan.Core.Models.Dtos;
using MediatR;

namespace Ewan.Application.Features.Auth.Commands.LoginAppUser
{
    public record LoginAppUserCommand(
        LoginRequestDto Request,
        string IpAddress
    ) : IRequest<AuthResponseDto>;
}
