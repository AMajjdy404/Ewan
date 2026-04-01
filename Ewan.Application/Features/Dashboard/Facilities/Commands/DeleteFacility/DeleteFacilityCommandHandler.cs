

using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ewan.Application.Features.Dashboard.Facilities.Commands.DeleteFacility
{

    public class DeleteFacilityCommandHandler : IRequestHandler<DeleteFacilityCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFacilityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteFacilityCommand command, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Facility>();

            var facility = await repo.GetByIdAsync(command.Id);

            if (facility == null)
                throw new KeyNotFoundException("Facility not found.");

            var isLinked = await _unitOfWork.Repository<PropertyFacility>()
                .AnyAsync(x => x.FacilityId == command.Id);

            if (isLinked)
                throw new BadHttpRequestException("Cannot delete facility because it is linked to properties.");

            repo.Delete(facility);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
