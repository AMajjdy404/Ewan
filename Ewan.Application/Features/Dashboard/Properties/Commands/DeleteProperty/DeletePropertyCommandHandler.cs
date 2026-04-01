using Ewan.Application.Helpers;
using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;

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

            foreach (var img in property.Images)
                DocumentSettings.DeleteFile(img.ImageUrl, "Properties");

            _unitOfWork.Repository<Property>().Delete(property);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
