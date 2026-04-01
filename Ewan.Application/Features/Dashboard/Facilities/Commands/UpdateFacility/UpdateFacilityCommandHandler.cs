

using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ewan.Application.Features.Dashboard.Facilities.Commands.UpdateFacility
{

    public class UpdateFacilityCommandHandler : IRequestHandler<UpdateFacilityCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFacilityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateFacilityCommand command, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Facility>();

            var facility = await repo.GetByIdAsync(command.Request.Id);

            if (facility == null)
                throw new KeyNotFoundException("Facility not found.");

            var newName = command.Request.Name.Trim();

            var exists = await repo.AnyAsync(x => x.Name == newName && x.Id != command.Request.Id);

            if (exists)
                throw new BadHttpRequestException("Facility name already exists.");

            facility.Name = newName;

            repo.Update(facility);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
