using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Faqs.Commands.UpdateFaq
{
    public class UpdateFaqCommandHandler : IRequestHandler<UpdateFaqCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFaqCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateFaqCommand command, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Faq>().GetByIdAsync(command.Request.Id);
            if (entity == null)
                throw new KeyNotFoundException("FAQ not found.");

            entity.Question = command.Request.Question.Trim();
            entity.Answer = command.Request.Answer.Trim();

            _unitOfWork.Repository<Faq>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
