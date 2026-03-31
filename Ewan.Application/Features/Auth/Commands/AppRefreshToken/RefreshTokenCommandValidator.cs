using Ewan.Application.Features.Auth.Commands.AppRefreshToken;
using FluentValidation;

namespace Ewan.Application.Features.Auth.Commands.AppRefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.Request.RefreshToken)
                .NotEmpty();
        }
    }
}