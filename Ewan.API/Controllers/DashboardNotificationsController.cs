using Ewan.API.Errors;
using Ewan.Application.Features.Dashboard.Notifications.Commands.SendClientsTopicNotification;
using Ewan.Core.Models.Dtos.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/dashboard/notifications")]
    [Authorize]
    public class DashboardNotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardNotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("clients")]
        public async Task<IActionResult> SendToClients([FromBody] SendClientsNotificationRequestDto request)
        {
            var messageId = await _mediator.Send(new SendClientsTopicNotificationCommand(request));
            return Ok(new ApiResponse(200, "Notification sent successfully.", new { messageId }));
        }
    }
}
