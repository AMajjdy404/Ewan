using FluentValidation;

namespace Ewan.Application.Features.Client.Profile.Commands.DeleteClientAccount
{
    public class DeleteClientAccountCommandValidator : AbstractValidator<DeleteClientAccountCommand>
    {
        public DeleteClientAccountCommandValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
        }
    }
}
