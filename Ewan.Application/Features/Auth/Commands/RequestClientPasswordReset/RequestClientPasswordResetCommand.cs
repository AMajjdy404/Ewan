using Ewan.Core.Models.Dtos;
using MediatR;

namespace Ewan.Application.Features.Auth.Commands.RequestClientPasswordReset
{
    public record RequestClientPasswordResetCommand(
        ForgotClientPasswordDto Request,
        string IpAddress
    ) : IRequest<Unit>;
}