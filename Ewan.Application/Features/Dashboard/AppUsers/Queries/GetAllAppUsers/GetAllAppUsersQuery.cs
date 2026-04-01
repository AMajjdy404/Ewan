using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.AppUser;
using MediatR;

namespace Ewan.Application.Features.Dashboard.AppUsers.Queries.GetAllAppUsers
{
    public record GetAllAppUsersQuery(PaginationParams Params) : IRequest<Pagination<AppUserDto>>;

}
