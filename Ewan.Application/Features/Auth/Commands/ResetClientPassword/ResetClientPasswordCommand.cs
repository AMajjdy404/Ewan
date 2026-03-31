using Ewan.Core.Models.Dtos;
using MediatR;

namespace Ewan.Application.Features.Auth.Commands.ResetClientPassword
{
    public record ResetClientPasswordCommand(
        ResetClientPasswordDto Request,
        string IpAddress
    ) : IRequest<Unit>;
}