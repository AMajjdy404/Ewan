using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Faqs.Commands.CreateFaq
{
    public class CreateFaqCommandHandler : IRequestHandler<CreateFaqCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFaqCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateFaqCommand command, CancellationToken cancellationToken)
        {
            var entity = new Faq
            {
                Question = command.Request.Question.Trim(),
                Answer = command.Request.Answer.Trim()
            };

            await _unitOfWork.Repository<Faq>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }
    }
}
