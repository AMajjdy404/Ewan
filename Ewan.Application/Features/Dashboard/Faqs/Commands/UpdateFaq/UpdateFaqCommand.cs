using Ewan.Core.Models.Dtos.Faq;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Faqs.Commands.UpdateFaq
{
    public record UpdateFaqCommand(UpdateFaqRequestDto Request) : IRequest;
}
