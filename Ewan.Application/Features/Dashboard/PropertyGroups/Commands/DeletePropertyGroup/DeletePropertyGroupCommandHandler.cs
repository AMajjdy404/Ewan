

using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
namespace Ewan.Application.Features.Dashboard.PropertyGroups.Commands.DeletePropertyGroup
{

    public class DeletePropertyGroupCommandHandler : IRequestHandler<DeletePropertyGroupCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePropertyGroupCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePropertyGroupCommand command, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<PropertyGroup>();
            var entity = await repo.GetByIdAsync(command.Id);

            if (entity == null)
                throw new KeyNotFoundException("Property group not found.");

            var hasProperties = await _unitOfWork.Repository<Property>()
                .AnyAsync(x => x.GroupId == command.Id);

            if (hasProperties)
                throw new BadHttpRequestException("Cannot delete property group because it is linked to properties.");

            repo.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
