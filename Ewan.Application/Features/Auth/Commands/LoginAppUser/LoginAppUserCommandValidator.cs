using FluentValidation;

namespace Ewan.Application.Features.Auth.Commands.LoginAppUser
{
    public class LoginAppUserCommandValidator : AbstractValidator<LoginAppUserCommand>
    {
        public LoginAppUserCommandValidator()
        {
            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Request.Password)
                .NotEmpty();
        }
    }
}
