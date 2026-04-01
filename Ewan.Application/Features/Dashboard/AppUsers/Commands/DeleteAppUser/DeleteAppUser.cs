using MediatR;

namespace Ewan.Application.Features.Dashboard.AppUsers.Commands.DeleteAppUser
{
    public record DeleteAppUserCommand(string Id) : IRequest<bool>;

}
