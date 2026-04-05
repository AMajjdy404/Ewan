using Ewan.Application.Helpers;
using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.DeleteProperty
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeletePropertyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePropertyCommand command, CancellationToken cancellationToken)
        {
            var spec = new PropertyWithDetailsSpecification(command.Id);
            var property = await _unitOfWork.Repository<Property>().GetEntityWithSpec(spec);

            if (property == null)
                throw new KeyNotFoundException("Property not found.");

            var hasBookings = await _unitOfWork.Repository<Booking>()
                .AnyAsync(x => x.PropertyId == command.Id);

            if (hasBookings)
                throw new BadHttpRequestException("Cannot delete property because it has related bookings.");

            var imageUrls = property.Images
                .Select(x => x.ImageUrl)
                .ToList();

            _unitOfWork.Repository<Property>().Delete(property);
            await _unitOfWork.SaveChangesAsync();

            foreach (var imageUrl in imageUrls)
                DocumentSettings.DeleteFile(imageUrl, "Properties");

            return true;
        }
    }
}
