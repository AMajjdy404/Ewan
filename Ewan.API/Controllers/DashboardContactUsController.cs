using Ewan.API.Errors;
using Ewan.Application.Features.Dashboard.ContactUs.Commands.UpsertContactUs;
using Ewan.Application.Features.Dashboard.ContactUs.Queries.GetContactUs;
using Ewan.Core.Models.Dtos.ContactUs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/dashboard/contact-us")]
    [Authorize]
    public class DashboardContactUsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardContactUsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetContactUsQuery());
            return Ok(new ApiResponse(200, "Contact us settings retrieved successfully.", result));
        }

        [HttpPut]
        public async Task<IActionResult> Upsert([FromBody] UpdateContactUsRequestDto request)
        {
            await _mediator.Send(new UpsertContactUsCommand(request));
            return Ok(new ApiResponse(200, "Contact us settings updated successfully."));
        }
    }
}
