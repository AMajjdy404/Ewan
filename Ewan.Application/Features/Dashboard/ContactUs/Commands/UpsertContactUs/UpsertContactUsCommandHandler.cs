using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;

namespace Ewan.Application.Features.Dashboard.ContactUs.Commands.UpsertContactUs
{
    public class UpsertContactUsCommandHandler : IRequestHandler<UpsertContactUsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertContactUsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpsertContactUsCommand command, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<ContactUsSetting>();
            var entity = await repo.FirstOrDefaultAsync(x => true);

            if (entity == null)
            {
                entity = new ContactUsSetting
                {
                    SupportNumber = command.Request.SupportNumber.Trim(),
                    WhatsappNumber = command.Request.WhatsappNumber.Trim(),
                    Email = command.Request.Email.Trim()
                };

                await repo.AddAsync(entity);
            }
            else
            {
                entity.SupportNumber = command.Request.SupportNumber.Trim();
                entity.WhatsappNumber = command.Request.WhatsappNumber.Trim();
                entity.Email = command.Request.Email.Trim();
                repo.Update(entity);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
