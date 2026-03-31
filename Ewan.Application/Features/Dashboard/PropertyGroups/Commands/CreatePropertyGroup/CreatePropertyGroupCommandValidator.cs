using FluentValidation;

namespace Ewan.Application.Features.Dashboard.PropertyGroups.Commands.CreatePropertyGroup
{

    public class CreatePropertyGroupCommandValidator : AbstractValidator<CreatePropertyGroupCommand>
    {
        public CreatePropertyGroupCommandValidator()
        {
            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
