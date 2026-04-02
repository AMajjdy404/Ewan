using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;

namespace Ewan.Application.Features.Client.Properties.Commands.RemoveFavoriteProperty
{
    public class RemoveFavoritePropertyCommandHandler : IRequestHandler<RemoveFavoritePropertyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFavoritePropertyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveFavoritePropertyCommand command, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<ClientFavoriteProperty>()
                .FirstOrDefaultAsync(x => x.ClientId == command.ClientId && x.PropertyId == command.PropertyId);

            if (entity == null)
                return;

            _unitOfWork.Repository<ClientFavoriteProperty>().Delete(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
