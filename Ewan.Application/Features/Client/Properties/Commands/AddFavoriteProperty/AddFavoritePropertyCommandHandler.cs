using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ewan.Application.Features.Client.Properties.Commands.AddFavoriteProperty
{
    public class AddFavoritePropertyCommandHandler : IRequestHandler<AddFavoritePropertyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddFavoritePropertyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddFavoritePropertyCommand command, CancellationToken cancellationToken)
        {
            var propertyExists = await _unitOfWork.Repository<Property>().AnyAsync(x => x.Id == command.PropertyId);
            if (!propertyExists)
                throw new KeyNotFoundException("Property not found.");

            var exists = await _unitOfWork.Repository<ClientFavoriteProperty>()
                .AnyAsync(x => x.ClientId == command.ClientId && x.PropertyId == command.PropertyId);

            if (exists)
                throw new BadHttpRequestException("Property is already in favorites.");

            await _unitOfWork.Repository<ClientFavoriteProperty>().AddAsync(new ClientFavoriteProperty
            {
                ClientId = command.ClientId,
                PropertyId = command.PropertyId,
                CreatedAt = DateTime.UtcNow
            });

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
