namespace Ewan.Application.Features.Auth.Commands.RegisterClient
{
    using FluentValidation;

    namespace Ewan.Application.Features.Auth.Commands.RegisterClient
    {
        public class RegisterClientCommandValidator : AbstractValidator<RegisterClientCommand>
        {
            public RegisterClientCommandValidator()
            {
                RuleFor(x => x.Request.FullName)
                    .NotEmpty()
                    .MaximumLength(150);

                RuleFor(x => x.Request.Email)
                    .NotEmpty()
                    .EmailAddress();

                RuleFor(x => x.Request.Password)
                    .NotEmpty()
                    .MinimumLength(6);

                RuleFor(x => x.Request.ConfirmPassword)
                    .NotEmpty()
                    .Equal(x => x.Request.Password)
                    .WithMessage("Passwords do not match.");

                RuleFor(x => x.Request.PhoneNumber)
                    .MaximumLength(20)
                    .When(x => !string.IsNullOrWhiteSpace(x.Request.PhoneNumber));
            }
        }
    }
}
