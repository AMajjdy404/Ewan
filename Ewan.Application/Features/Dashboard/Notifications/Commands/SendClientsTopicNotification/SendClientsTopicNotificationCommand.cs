using Ewan.Core.Models.Dtos.Notification;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Notifications.Commands.SendClientsTopicNotification
{
    public record SendClientsTopicNotificationCommand(SendClientsNotificationRequestDto Request) : IRequest<string>;
}
