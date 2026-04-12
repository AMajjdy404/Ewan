using Ewan.API.Errors;
using Ewan.Application.Features.Dashboard.TermsAndConditions.Commands.UpsertTermsAndConditions;
using Ewan.Application.Features.Dashboard.TermsAndConditions.Queries.GetTermsAndConditions;
using Ewan.Core.Models.Dtos.TermsAndConditions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/dashboard/terms-and-conditions")]
    [Authorize]
    public class DashboardTermsAndConditionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardTermsAndConditionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetTermsAndConditionsQuery());
            return Ok(new ApiResponse(200, "Terms and conditions retrieved successfully.", result));
        }

        [HttpPut]
        public async Task<IActionResult> Upsert([FromBody] UpdateTermsAndConditionsRequestDto request)
        {
            await _mediator.Send(new UpsertTermsAndConditionsCommand(request));
            return Ok(new ApiResponse(200, "Terms and conditions updated successfully."));
        }
    }
}
