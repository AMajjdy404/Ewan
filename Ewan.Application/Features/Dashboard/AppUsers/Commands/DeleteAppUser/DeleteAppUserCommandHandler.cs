using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Ewan.Application.Features.Dashboard.AppUsers.Commands.DeleteAppUser
{
    public class DeleteAppUserCommandHandler : IRequestHandler<DeleteAppUserCommand, bool>
    {
        private readonly UserManager<AppUser> _userManager;
        public DeleteAppUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeleteAppUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.Id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new BadHttpRequestException(string.Join(" | ", result.Errors.Select(e => e.Description)));

            return true;
        }
    }
}
