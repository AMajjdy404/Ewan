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

            RuleFor(x => x.Request.DeviceToken)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Request.DeviceToken));
        }
    }
}