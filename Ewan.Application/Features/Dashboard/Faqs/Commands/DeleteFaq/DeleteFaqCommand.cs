using MediatR;

namespace Ewan.Application.Features.Dashboard.Faqs.Commands.DeleteFaq
{
    public record DeleteFaqCommand(int Id) : IRequest;
}
