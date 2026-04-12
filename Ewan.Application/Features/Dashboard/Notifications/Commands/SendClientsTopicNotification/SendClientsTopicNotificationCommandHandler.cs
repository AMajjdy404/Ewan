using Ewan.Core.Services;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Notifications.Commands.SendClientsTopicNotification
{
    public class SendClientsTopicNotificationCommandHandler : IRequestHandler<SendClientsTopicNotificationCommand, string>
    {
        private readonly IFirebaseNotificationService _firebaseNotificationService;

        public SendClientsTopicNotificationCommandHandler(IFirebaseNotificationService firebaseNotificationService)
        {
            _firebaseNotificationService = firebaseNotificationService;
        }

        public async Task<string> Handle(SendClientsTopicNotificationCommand command, CancellationToken cancellationToken)
        {
            return await _firebaseNotificationService.SendToAllClientsTopicAsync(
                command.Request.Title.Trim(),
                command.Request.Body.Trim(),
                cancellationToken);
        }
    }
}
