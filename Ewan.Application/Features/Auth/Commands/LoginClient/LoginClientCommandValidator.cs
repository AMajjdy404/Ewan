using FluentValidation;

namespace Ewan.Application.Features.Auth.Commands.LoginClient
{
    public class LoginClientCommandValidator : AbstractValidator<LoginClientCommand>
    {
        public LoginClientCommandValidator()
        {
            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Request.Password)
                .NotEmpty();
        }
    }
}