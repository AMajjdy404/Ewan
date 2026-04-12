using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Faq;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Faqs.Queries.GetAllFaqs
{
    public class GetAllFaqsQueryHandler : IRequestHandler<GetAllFaqsQuery, IReadOnlyList<FaqDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllFaqsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<FaqDto>> Handle(GetAllFaqsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Repository<Faq>().ListAllAsync();

            return entities
                .OrderByDescending(x => x.Id)
                .Select(x => new FaqDto
                {
                    Id = x.Id,
                    Question = x.Question,
                    Answer = x.Answer
                })
                .ToList();
        }
    }
}
