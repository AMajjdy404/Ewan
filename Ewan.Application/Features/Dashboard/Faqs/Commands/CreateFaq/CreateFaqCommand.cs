using Ewan.Core.Models.Dtos.Faq;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Faqs.Commands.CreateFaq
{
    public record CreateFaqCommand(CreateFaqRequestDto Request) : IRequest<int>;
}
