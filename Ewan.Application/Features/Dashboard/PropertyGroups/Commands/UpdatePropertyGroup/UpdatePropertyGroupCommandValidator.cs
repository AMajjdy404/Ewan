 using FluentValidation;

namespace Ewan.Application.Features.Dashboard.PropertyGroups.Commands.UpdatePropertyGroup
{

    public class UpdatePropertyGroupCommandValidator : AbstractValidator<UpdatePropertyGroupCommand>
    {
        public UpdatePropertyGroupCommandValidator()
        {
            RuleFor(x => x.Request.Id)
                .GreaterThan(0);

            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
