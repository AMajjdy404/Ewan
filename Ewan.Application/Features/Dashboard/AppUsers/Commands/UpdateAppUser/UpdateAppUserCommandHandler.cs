using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

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
            var userId = command.Request.Id.Trim();
            var email = command.Request.Email.Trim();
            var userName = command.Request.UserName.Trim();
            var phoneNumber = command.Request.PhoneNumber.Trim();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (!await _roleManager.RoleExistsAsync(command.Request.Role))
                throw new BadHttpRequestException($"Role '{command.Request.Role}' does not exist.");

            var existingByEmail = await _userManager.FindByEmailAsync(email);
            if (existingByEmail != null && existingByEmail.Id != user.Id)
                throw new BadHttpRequestException("Email already exists.");

            var existingByUserName = await _userManager.FindByNameAsync(userName);
            if (existingByUserName != null && existingByUserName.Id != user.Id)
                throw new BadHttpRequestException("Username already exists.");

            user.UserName = userName;
            user.Email = email;
            user.PhoneNumber = phoneNumber;

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
