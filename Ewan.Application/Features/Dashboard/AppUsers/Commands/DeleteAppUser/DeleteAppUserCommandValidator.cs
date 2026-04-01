using FluentValidation;

namespace Ewan.Application.Features.Dashboard.AppUsers.Commands.DeleteAppUser
{
    public class DeleteAppUserCommandValidator : AbstractValidator<DeleteAppUserCommand>
    {
        public DeleteAppUserCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
