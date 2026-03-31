using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ewan.Application.Features.Dashboard.PropertyGroups.Commands.UpdatePropertyGroup
{

    public class UpdatePropertyGroupCommandHandler : IRequestHandler<UpdatePropertyGroupCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePropertyGroupCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdatePropertyGroupCommand command, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<PropertyGroup>();
            var entity = await repo.GetByIdAsync(command.Request.Id);

            if (entity == null)
                throw new KeyNotFoundException("Property group not found.");

            var newName = command.Request.Name.Trim();

            var exists = await repo.AnyAsync(x => x.Name == newName && x.Id != command.Request.Id);

            if (exists)
                throw new BadHttpRequestException("Property group name already exists.");

            entity.Name = newName;

            repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
