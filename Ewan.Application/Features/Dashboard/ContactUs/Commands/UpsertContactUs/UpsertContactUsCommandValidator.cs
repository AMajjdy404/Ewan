using FluentValidation;

namespace Ewan.Application.Features.Dashboard.ContactUs.Commands.UpsertContactUs
{
    public class UpsertContactUsCommandValidator : AbstractValidator<UpsertContactUsCommand>
    {
        public UpsertContactUsCommandValidator()
        {
            RuleFor(x => x.Request.SupportNumber).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Request.WhatsappNumber).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Request.Email).NotEmpty().EmailAddress().MaximumLength(256);
        }
    }
}
