using Ewan.Core.Models.Dtos.AppUser;
using MediatR;

namespace Ewan.Application.Features.Dashboard.AppUsers.Commands.UpdateAppUserPassword
{
    public record UpdateAppUserPasswordCommand(string UserId, UpdateAppUserPasswordRequestDto Request) : IRequest;
}
