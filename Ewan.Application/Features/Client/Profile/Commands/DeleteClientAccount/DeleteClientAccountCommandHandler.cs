using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ewan.Application.Features.Client.Profile.Commands.DeleteClientAccount
{
    public class DeleteClientAccountCommandHandler : IRequestHandler<DeleteClientAccountCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteClientAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteClientAccountCommand request, CancellationToken cancellationToken)
        {
            var clientRepo = _unitOfWork.Repository<global::Ewan.Core.Models.Client>();
            var client = await clientRepo.GetByIdAsync(request.ClientId);
            if (client == null)
                throw new KeyNotFoundException("Client not found.");

            if (client.IsDeleted)
                return;

            var hasActiveOrUpcomingBookings = await _unitOfWork.Repository<Booking>()
                .AnyAsync(x =>
                    x.ClientId == request.ClientId &&
                    x.Status != BookingStatus.Cancelled &&
                    x.CheckOutDate.Date >= DateTime.UtcNow.Date);

            if (hasActiveOrUpcomingBookings)
                throw new BadHttpRequestException("Cannot delete account while there are active or upcoming bookings.");

            await _unitOfWork.Repository<RefreshToken>()
                .ExecuteDeleteAsync(x => x.OwnerId == request.ClientId.ToString() && x.UserType == "Client", cancellationToken);

            await _unitOfWork.Repository<ClientPasswordResetToken>()
                .ExecuteDeleteAsync(x => x.ClientId == request.ClientId, cancellationToken);

            client.IsDeleted = true;
            client.DeletedAt = DateTime.UtcNow;
            clientRepo.Update(client);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
