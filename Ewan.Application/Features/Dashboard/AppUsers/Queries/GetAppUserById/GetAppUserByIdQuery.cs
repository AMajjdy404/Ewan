using Ewan.Core.Models.Dtos.AppUser;
using MediatR;

namespace Ewan.Application.Features.Dashboard.AppUsers.Queries.GetAppUserById
{
    public record GetAppUserByIdQuery(string Id) : IRequest<AppUserDto>;

}
