using FluentValidation;

namespace Ewan.Application.Features.Dashboard.PropertyGroups.Commands.DeletePropertyGroup
{

    public class DeletePropertyGroupCommandValidator : AbstractValidator<DeletePropertyGroupCommand>
    {
        public DeletePropertyGroupCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
