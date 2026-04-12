using FluentValidation;

namespace Ewan.Application.Features.Dashboard.Notifications.Commands.SendClientsTopicNotification
{
    public class SendClientsTopicNotificationCommandValidator : AbstractValidator<SendClientsTopicNotificationCommand>
    {
        public SendClientsTopicNotificationCommandValidator()
        {
            RuleFor(x => x.Request.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Request.Body).NotEmpty().MaximumLength(1000);
        }
    }
}
