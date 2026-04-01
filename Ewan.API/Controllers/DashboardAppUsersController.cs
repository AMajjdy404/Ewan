using Ewan.API.Errors;
using Ewan.Application.Features.Dashboard.AppUsers.Commands.CreateAppUser;
using Ewan.Application.Features.Dashboard.AppUsers.Commands.DeleteAppUser;
using Ewan.Application.Features.Dashboard.AppUsers.Commands.UpdateAppUser;
using Ewan.Application.Features.Dashboard.AppUsers.Queries.GetAllAppUsers;
using Ewan.Application.Features.Dashboard.AppUsers.Queries.GetAppUserById;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.AppUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/dashboard/users")]
    [Authorize]
    public class DashboardAppUsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DashboardAppUsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams param)
        {
            var result = await _mediator.Send(new GetAllAppUsersQuery(param));
            return Ok(new ApiResponse(200, "Users retrieved successfully.", result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetAppUserByIdQuery(id));
            return Ok(new ApiResponse(200, "User retrieved successfully.", result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppUserRequestDto request)
        {
            var id = await _mediator.Send(new CreateAppUserCommand(request));
            return Ok(new ApiResponse(201, "User created successfully.", id));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAppUserRequestDto request)
        {
            await _mediator.Send(new UpdateAppUserCommand(request));
            return Ok(new ApiResponse(200, "User updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteAppUserCommand(id));
            return Ok(new ApiResponse(200, "User deleted successfully."));
        }
    }
}
