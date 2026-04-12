using Ewan.Core.Services;
using FirebaseAdmin.Messaging;

namespace Ewan.Application
{
    public class FirebaseNotificationService : IFirebaseNotificationService
    {
        private const string AllClientsTopic = "all_clients";

        public async Task SubscribeTokenToAllClientsTopicAsync(string deviceToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(deviceToken))
                return;

            await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(new List<string> { deviceToken }, AllClientsTopic);
        }

        public async Task<string> SendToAllClientsTopicAsync(string title, string body, CancellationToken cancellationToken = default)
        {
            var message = new Message
            {
                Topic = AllClientsTopic,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
        }
    }
}
