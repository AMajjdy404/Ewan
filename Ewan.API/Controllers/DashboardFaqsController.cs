using Ewan.API.Errors;
using Ewan.Application.Features.Dashboard.Faqs.Commands.CreateFaq;
using Ewan.Application.Features.Dashboard.Faqs.Commands.DeleteFaq;
using Ewan.Application.Features.Dashboard.Faqs.Commands.UpdateFaq;
using Ewan.Application.Features.Dashboard.Faqs.Queries.GetAllFaqs;
using Ewan.Core.Models.Dtos.Faq;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/dashboard/faqs")]
    [Authorize]
    public class DashboardFaqsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardFaqsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllFaqsQuery());
            return Ok(new ApiResponse(200, "FAQs retrieved successfully.", result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFaqRequestDto request)
        {
            var id = await _mediator.Send(new CreateFaqCommand(request));
            return Ok(new ApiResponse(201, "FAQ created successfully.", id));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateFaqRequestDto request)
        {
            await _mediator.Send(new UpdateFaqCommand(request));
            return Ok(new ApiResponse(200, "FAQ updated successfully."));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteFaqCommand(id));
            return Ok(new ApiResponse(200, "FAQ deleted successfully."));
        }
    }
}
