using Ewan.API.Errors;
using Ewan.Application.Features.Dashboard.Facilities.Commands.CreateFacility;
using Ewan.Application.Features.Dashboard.Facilities.Commands.DeleteFacility;
using Ewan.Application.Features.Dashboard.Facilities.Commands.UpdateFacility;
using Ewan.Application.Features.Dashboard.Facilities.Queries.GetAllFacilities;
using Ewan.Application.Features.Dashboard.Facilities.Queries.GetFacilityById;
using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Facility;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/dashboard/facilities")]
    [Authorize]
    public class DashboardFacilitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardFacilitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams param)
        {
            var result = await _mediator.Send(new GetAllFacilitiesQuery(param));
            return Ok(new ApiResponse(200, "Facilities retrieved successfully.", result));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetFacilityByIdQuery(id));
            return Ok(new ApiResponse(200, "Facility retrieved successfully.", result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFacilityRequestDto request)
        {
            var id = await _mediator.Send(new CreateFacilityCommand(request));
            return Ok(new ApiResponse(201, "Facility created successfully.", id));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateFacilityRequestDto request)
        {
            await _mediator.Send(new UpdateFacilityCommand(request));
            return Ok(new ApiResponse(200, "Facility updated successfully."));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteFacilityCommand(id));
            return Ok(new ApiResponse(200, "Facility deleted successfully."));
        }
    }
}
