using FluentValidation;

namespace Ewan.Application.Features.Auth.Commands.ResetClientPassword
{
    public class ResetClientPasswordCommandValidator : AbstractValidator<ResetClientPasswordCommand>
    {
        public ResetClientPasswordCommandValidator()
        {
            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Request.Code)
                .NotEmpty();

            RuleFor(x => x.Request.NewPassword)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.Request.ConfirmNewPassword)
                .Equal(x => x.Request.NewPassword)
                .WithMessage("Passwords do not match.");
        }
    }
}