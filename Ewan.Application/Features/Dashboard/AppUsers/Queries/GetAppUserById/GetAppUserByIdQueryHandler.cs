using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.AppUser;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ewan.Application.Features.Dashboard.AppUsers.Queries.GetAppUserById
{
    public class GetAppUserByIdQueryHandler : IRequestHandler<GetAppUserByIdQuery, AppUserDto>
    {
        private readonly UserManager<AppUser> _userManager;
        public GetAppUserByIdQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUserDto> Handle(GetAppUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var roles = await _userManager.GetRolesAsync(user);

            return new AppUserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,
                Roles = roles
            };
        }
    }
}
