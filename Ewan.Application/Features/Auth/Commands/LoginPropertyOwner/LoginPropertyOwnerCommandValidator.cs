using FluentValidation;

namespace Ewan.Application.Features.Auth.Commands.LoginPropertyOwner
{
    public class LoginPropertyOwnerCommandValidator : AbstractValidator<LoginPropertyOwnerCommand>
    {
        public LoginPropertyOwnerCommandValidator()
        {
            RuleFor(x => x.Request.PhoneNumber).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Request.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
        }
    }
}
