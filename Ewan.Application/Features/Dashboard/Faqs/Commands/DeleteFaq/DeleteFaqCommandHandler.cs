using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Faqs.Commands.DeleteFaq
{
    public class DeleteFaqCommandHandler : IRequestHandler<DeleteFaqCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFaqCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteFaqCommand command, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<Faq>().GetByIdAsync(command.Id);
            if (entity == null)
                throw new KeyNotFoundException("FAQ not found.");

            _unitOfWork.Repository<Faq>().Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
