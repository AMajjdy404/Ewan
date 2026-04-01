using FluentValidation;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.DeleteProperty
{
    public class DeletePropertyCommandValidator : AbstractValidator<DeletePropertyCommand>
    {
        public DeletePropertyCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
