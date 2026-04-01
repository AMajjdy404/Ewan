using Ewan.API.Errors;
using Ewan.Application.Features.Dashboard.Properties.Commands.CreateProperty;
using Ewan.Application.Features.Dashboard.Properties.Commands.DeleteProperty;
using Ewan.Application.Features.Dashboard.Properties.Commands.UpdateProperty;
using Ewan.Application.Features.Dashboard.Properties.Queries.GetAllProperties;
using Ewan.Application.Features.Dashboard.Properties.Queries.GetPropertyById;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Property;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/dashboard/properties")]
    [Authorize]
    public class DashboardPropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DashboardPropertiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams param)
        {
            var result = await _mediator.Send(new GetAllPropertiesQuery(param));
            return Ok(new ApiResponse(200, "Properties retrieved successfully.", result));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetPropertyByIdQuery(id));
            return Ok(new ApiResponse(200, "Property retrieved successfully.", result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePropertyRequestDto request)
        {
            var id = await _mediator.Send(new CreatePropertyCommand(request));
            return Ok(new ApiResponse(201, "Property created successfully.", id));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdatePropertyRequestDto request)
        {
            await _mediator.Send(new UpdatePropertyCommand(request));
            return Ok(new ApiResponse(200, "Property updated successfully."));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeletePropertyCommand(id));
            return Ok(new ApiResponse(200, "Property deleted successfully."));
        }
    }
}
