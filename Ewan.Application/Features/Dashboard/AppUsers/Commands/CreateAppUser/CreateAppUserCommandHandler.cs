using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Ewan.Application.Features.Dashboard.AppUsers.Commands.CreateAppUser
{
    public class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand, string>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CreateAppUserCommandHandler(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> Handle(CreateAppUserCommand command, CancellationToken cancellationToken)
        {
            var existingEmail = await _userManager.FindByEmailAsync(command.Request.Email);
            if (existingEmail != null)
                throw new BadHttpRequestException("Email already exists.");

            var existingUserName = await _userManager.FindByNameAsync(command.Request.UserName);
            if (existingUserName != null)
                throw new BadHttpRequestException("Username already exists.");

            if (!await _roleManager.RoleExistsAsync(command.Request.Role))
                throw new BadHttpRequestException($"Role '{command.Request.Role}' does not exist.");

            var user = new AppUser
            {
                UserName = command.Request.UserName.Trim(),
                Email = command.Request.Email.Trim(),
                PhoneNumber = command.Request.PhoneNumber.Trim()
            };

            var result = await _userManager.CreateAsync(user, command.Request.Password);
            if (!result.Succeeded)
                throw new BadHttpRequestException(string.Join(" | ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, command.Request.Role);

            return user.Id;
        }
    }
}
