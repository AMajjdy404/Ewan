using Ewan.API.Errors;
using Ewan.Application.Features.Dashboard.PropertyGroups.Commands.CreatePropertyGroup;
using Ewan.Application.Features.Dashboard.PropertyGroups.Commands.DeletePropertyGroup;
using Ewan.Application.Features.Dashboard.PropertyGroups.Commands.UpdatePropertyGroup;
using Ewan.Application.Features.Dashboard.PropertyGroups.Queries.GetAllPropertyGroups;
using Ewan.Application.Features.Dashboard.PropertyGroups.Queries.GetPropertyGroupById;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.PropertyGroup;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ewan.API.Controllers
{

    [ApiController]
    [Route("api/dashboard/property-groups")]
    [Authorize]
    public class DashboardPropertyGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardPropertyGroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams param)
        {
            var result = await _mediator.Send(new GetAllPropertyGroupsQuery(param));

            return Ok(new ApiResponse(200, "Property groups retrieved successfully.", result));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetPropertyGroupByIdQuery(id));
            return Ok(new ApiResponse(200, "Property group retrieved successfully.", result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePropertyGroupRequestDto request)
        {
            var id = await _mediator.Send(new CreatePropertyGroupCommand(request));
            return Ok(new ApiResponse(201, "Property group created successfully.", id));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePropertyGroupRequestDto request)
        {
            await _mediator.Send(new UpdatePropertyGroupCommand(request));
            return Ok(new ApiResponse(200, "Property group updated successfully."));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeletePropertyGroupCommand(id));
            return Ok(new ApiResponse(200, "Property group deleted successfully."));
        }
    }
}
