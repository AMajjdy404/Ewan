using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ewan.Application.Features.Dashboard.AppUsers.Commands.UpdateAppUser
{
    public class UpdateAppUserCommandHandler : IRequestHandler<UpdateAppUserCommand, bool>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UpdateAppUserCommandHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(UpdateAppUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.Request.Id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (!await _roleManager.RoleExistsAsync(command.Request.Role))
                throw new BadHttpRequestException($"Role '{command.Request.Role}' does not exist.");

            var emailExists = await _userManager.Users
                .AnyAsync(u => u.Email == command.Request.Email && u.Id != command.Request.Id, cancellationToken);
            if (emailExists)
                throw new BadHttpRequestException("Email already exists.");

            var userNameExists = await _userManager.Users
                .AnyAsync(u => u.UserName == command.Request.UserName && u.Id != command.Request.Id, cancellationToken);
            if (userNameExists)
                throw new BadHttpRequestException("Username already exists.");

            user.UserName = command.Request.UserName.Trim();
            user.Email = command.Request.Email.Trim();
            user.PhoneNumber = command.Request.PhoneNumber.Trim();

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                throw new BadHttpRequestException(string.Join(" | ", updateResult.Errors.Select(e => e.Description)));

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, command.Request.Role);

            return true;
        }
    }
}
