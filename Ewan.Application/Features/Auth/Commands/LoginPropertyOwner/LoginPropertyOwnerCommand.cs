using Ewan.Core.Models.Dtos;
using MediatR;

namespace Ewan.Application.Features.Auth.Commands.LoginPropertyOwner
{
    public record LoginPropertyOwnerCommand(PropertyOwnerLoginRequestDto Request) : IRequest<AuthResponseDto>;
}
