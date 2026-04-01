using FluentValidation;

namespace Ewan.Application.Features.Dashboard.AppUsers.Commands.CreateAppUser
{
    public class CreateAppUserCommandValidator : AbstractValidator<CreateAppUserCommand>
    {
        public CreateAppUserCommandValidator()
        {
            RuleFor(x => x.Request.UserName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Request.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Request.PhoneNumber).NotEmpty();
            RuleFor(x => x.Request.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Request.Role).NotEmpty();
        }
    }
}
