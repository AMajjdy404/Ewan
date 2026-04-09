using Ewan.API.Errors;
using Ewan.Application.Features.Dashboard.Clients.Commands.UpdateClientActivation;
using Ewan.Application.Features.Dashboard.Clients.Queries.GetAllClients;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Client;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/dashboard/clients")]
    [Authorize]
    public class DashboardClientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardClientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams param)
        {
            var result = await _mediator.Send(new GetAllClientsQuery(param));
            return Ok(new ApiResponse(200, "Clients retrieved successfully.", result));
        }

        [HttpPatch("{id:int}/activation")]
        public async Task<IActionResult> UpdateActivation(int id, [FromBody] UpdateClientActivationRequestDto request)
        {
            await _mediator.Send(new UpdateClientActivationCommand(id, request.IsActive));
            return Ok(new ApiResponse(200, "Client activation updated successfully."));
        }
    }
}
