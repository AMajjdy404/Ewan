using Ewan.Core.Models;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.AppUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ewan.Application.Features.Dashboard.AppUsers.Queries.GetAllAppUsers
{
    public class GetAllAppUsersQueryHandler : IRequestHandler<GetAllAppUsersQuery, Pagination<AppUserDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        public GetAllAppUsersQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Pagination<AppUserDto>> Handle(GetAllAppUsersQuery request, CancellationToken cancellationToken)
        {
            var allUsers = _userManager.Users.OrderBy(u => u.UserName);

            var totalCount = await allUsers.CountAsync(cancellationToken);

            var users = await allUsers
                .Skip((request.Params.PageIndex - 1) * request.Params.PageSize)
                .Take(request.Params.PageSize)
                .ToListAsync(cancellationToken);

            var data = new List<AppUserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                data.Add(new AppUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber!,
                    Roles = roles
                });
            }

            return new Pagination<AppUserDto>(
                request.Params.PageIndex,
                request.Params.PageSize,
                totalCount,
                data
            );
        }
    }
}
