using FluentValidation;

namespace Ewan.Application.Features.Dashboard.Clients.Commands.UpdateClientActivation
{
    public class UpdateClientActivationCommandValidator : AbstractValidator<UpdateClientActivationCommand>
    {
        public UpdateClientActivationCommandValidator()
        {
            RuleFor(x => x.ClientId).GreaterThan(0);
        }
    }
}
