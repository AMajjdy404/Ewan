using Ewan.Core.Interfaces;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Clients.Commands.UpdateClientActivation
{
    public class UpdateClientActivationCommandHandler : IRequestHandler<UpdateClientActivationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateClientActivationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateClientActivationCommand request, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.Repository<global::Ewan.Core.Models.Client>().GetByIdAsync(request.ClientId);
            if (client == null || client.IsDeleted)
                throw new KeyNotFoundException("Client not found.");

            client.IsActive = request.IsActive;
            _unitOfWork.Repository<global::Ewan.Core.Models.Client>().Update(client);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
