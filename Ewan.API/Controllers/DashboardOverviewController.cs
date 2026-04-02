using Ewan.API.Errors;
using Ewan.Application.Features.Dashboard.Overview.Queries.GetDashboardCharts;
using Ewan.Application.Features.Dashboard.Overview.Queries.GetDashboardKpis;
using Ewan.Application.Features.Dashboard.Overview.Queries.GetDashboardLists;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/dashboard/overview")]
    [Authorize]
    public class DashboardOverviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardOverviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("kpis")]
        public async Task<IActionResult> GetKpis()
        {
            var result = await _mediator.Send(new GetDashboardKpisQuery());
            return Ok(new ApiResponse(200, "Dashboard KPIs retrieved successfully.", result));
        }

        [HttpGet("charts")]
        public async Task<IActionResult> GetCharts([FromQuery] int months = 6)
        {
            var result = await _mediator.Send(new GetDashboardChartsQuery(months));
            return Ok(new ApiResponse(200, "Dashboard charts retrieved successfully.", result));
        }

        [HttpGet("lists")]
        public async Task<IActionResult> GetLists([FromQuery] int topCount = 5, [FromQuery] int recentCount = 5)
        {
            var result = await _mediator.Send(new GetDashboardListsQuery(topCount, recentCount));
            return Ok(new ApiResponse(200, "Dashboard lists retrieved successfully.", result));
        }
    }
}
