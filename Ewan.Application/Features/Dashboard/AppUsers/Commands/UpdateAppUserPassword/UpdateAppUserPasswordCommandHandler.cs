using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Ewan.Application.Features.Dashboard.AppUsers.Commands.UpdateAppUserPassword
{
    public class UpdateAppUserPasswordCommandHandler : IRequestHandler<UpdateAppUserPasswordCommand>
    {
        private readonly UserManager<AppUser> _userManager;

        public UpdateAppUserPasswordCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(UpdateAppUserPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, command.Request.NewPassword);

            if (!result.Succeeded)
                throw new BadHttpRequestException(string.Join(" | ", result.Errors.Select(e => e.Description)));
        }
    }
}
