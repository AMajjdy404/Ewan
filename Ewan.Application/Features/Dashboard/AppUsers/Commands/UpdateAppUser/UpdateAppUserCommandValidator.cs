using FluentValidation;

namespace Ewan.Application.Features.Dashboard.AppUsers.Commands.UpdateAppUser
{
    public class UpdateAppUserCommandValidator : AbstractValidator<UpdateAppUserCommand>
    {
        public UpdateAppUserCommandValidator()
        {
            RuleFor(x => x.Request.Id).NotEmpty();
            RuleFor(x => x.Request.UserName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Request.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Request.PhoneNumber).NotEmpty();
            RuleFor(x => x.Request.Role).NotEmpty();
        }
    }
}
