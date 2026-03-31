using Ewan.Core.Models.Dtos;
using MediatR;

namespace Ewan.Application.Features.Auth.Commands.AppRefreshToken
{
    public record RefreshTokenCommand(
        RefreshTokenRequestDto Request,
        string IpAddress
    ) : IRequest<AuthResponseDto>;
}