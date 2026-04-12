using Ewan.Application.Helpers;
using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;

namespace Ewan.Application.Features.Dashboard.TermsAndConditions.Commands.UpsertTermsAndConditions
{
    public class UpsertTermsAndConditionsCommandHandler : IRequestHandler<UpsertTermsAndConditionsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertTermsAndConditionsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpsertTermsAndConditionsCommand command, CancellationToken cancellationToken)
        {
            var sanitizedHtml = TextToHtmlConverter.ConvertPlainTextToHtml(command.Request.Content);

            var repo = _unitOfWork.Repository<TermsAndConditionsSetting>();
            var entity = await repo.FirstOrDefaultAsync(x => true);

            if (entity == null)
            {
                entity = new TermsAndConditionsSetting
                {
                    Content = sanitizedHtml
                };

                await repo.AddAsync(entity);
            }
            else
            {
                entity.Content = sanitizedHtml;
                repo.Update(entity);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
