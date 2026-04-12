using Ewan.API.Errors;
using Ewan.Application.Features.Dashboard.ContactUs.Queries.GetContactUs;
using Ewan.Application.Features.Dashboard.Faqs.Queries.GetAllFaqs;
using Ewan.Application.Features.Dashboard.TermsAndConditions.Queries.GetTermsAndConditions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ewan.API.Controllers
{
    [ApiController]
    [Route("api/client/content")]
    public class ClientContentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientContentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("contact-us")]
        public async Task<IActionResult> GetContactUs()
        {
            var result = await _mediator.Send(new GetContactUsQuery());
            return Ok(new ApiResponse(200, "Contact us retrieved successfully.", result));
        }

        [AllowAnonymous]
        [HttpGet("faqs")]
        public async Task<IActionResult> GetFaqs()
        {
            var result = await _mediator.Send(new GetAllFaqsQuery());
            return Ok(new ApiResponse(200, "FAQs retrieved successfully.", result));
        }

        [AllowAnonymous]
        [HttpGet("terms-and-conditions")]
        public async Task<IActionResult> GetTermsAndConditions()
        {
            var result = await _mediator.Send(new GetTermsAndConditionsQuery());
            return Ok(new ApiResponse(200, "Terms and conditions retrieved successfully.", result));
        }
    }
}
