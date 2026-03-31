using FluentValidation;

namespace Ewan.Application.Features.Auth.Commands.RequestClientPasswordReset
{
    public class RequestClientPasswordResetCommandValidator : AbstractValidator<RequestClientPasswordResetCommand>
    {
        public RequestClientPasswordResetCommandValidator()
        {
            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}