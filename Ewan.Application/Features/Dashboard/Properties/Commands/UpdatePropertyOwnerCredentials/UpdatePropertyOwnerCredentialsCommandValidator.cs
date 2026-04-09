using FluentValidation;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.UpdatePropertyOwnerCredentials
{
    public class UpdatePropertyOwnerCredentialsCommandValidator : AbstractValidator<UpdatePropertyOwnerCredentialsCommand>
    {
        public UpdatePropertyOwnerCredentialsCommandValidator()
        {
            RuleFor(x => x.PropertyId).GreaterThan(0);
            RuleFor(x => x.Request.OwnerPhoneNumber).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Request.OwnerPassword)
                .MinimumLength(6)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Request.OwnerPassword));
        }
    }
}
