using Ewan.Core.Models.Dtos.Faq;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Faqs.Queries.GetAllFaqs
{
    public record GetAllFaqsQuery : IRequest<IReadOnlyList<FaqDto>>;
}
