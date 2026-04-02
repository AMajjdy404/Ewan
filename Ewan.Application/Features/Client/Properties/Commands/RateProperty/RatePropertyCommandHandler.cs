using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;

namespace Ewan.Application.Features.Client.Properties.Commands.RateProperty
{
    public class RatePropertyCommandHandler : IRequestHandler<RatePropertyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RatePropertyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RatePropertyCommand command, CancellationToken cancellationToken)
        {
            var propertyExists = await _unitOfWork.Repository<Property>().AnyAsync(x => x.Id == command.PropertyId);
            if (!propertyExists)
                throw new KeyNotFoundException("Property not found.");

            var ratingRepo = _unitOfWork.Repository<PropertyRating>();
            var existing = await ratingRepo.FirstOrDefaultAsync(x => x.ClientId == command.ClientId && x.PropertyId == command.PropertyId);

            if (existing == null)
            {
                await ratingRepo.AddAsync(new PropertyRating
                {
                    ClientId = command.ClientId,
                    PropertyId = command.PropertyId,
                    Rate = command.Request.Rate,
                    Comment = command.Request.Comment?.Trim(),
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                existing.Rate = command.Request.Rate;
                existing.Comment = command.Request.Comment?.Trim();
                existing.UpdatedAt = DateTime.UtcNow;
                ratingRepo.Update(existing);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
