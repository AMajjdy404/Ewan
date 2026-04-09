using FluentValidation;

namespace Ewan.Application.Features.Dashboard.AppUsers.Commands.UpdateAppUserPassword
{
    public class UpdateAppUserPasswordCommandValidator : AbstractValidator<UpdateAppUserPasswordCommand>
    {
        public UpdateAppUserPasswordCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Request.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(100);
        }
    }
}
