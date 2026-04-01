using Ewan.Core.Models.Dtos.AppUser;
using MediatR;

namespace Ewan.Application.Features.Dashboard.AppUsers.Commands.CreateAppUser
{
    public record CreateAppUserCommand(CreateAppUserRequestDto Request) : IRequest<string>;

}
