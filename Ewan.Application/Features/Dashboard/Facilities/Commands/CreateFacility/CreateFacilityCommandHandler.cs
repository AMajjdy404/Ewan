using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ewan.Application.Features.Dashboard.Facilities.Commands.CreateFacility
{
    public class CreateFacilityCommandHandler : IRequestHandler<CreateFacilityCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFacilityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateFacilityCommand command, CancellationToken cancellationToken)
        {
            var name = command.Request.Name.Trim();

            var exists = await _unitOfWork.Repository<Facility>()
                .AnyAsync(x => x.Name == name);

            if (exists)
                throw new BadHttpRequestException("Facility name already exists.");

            var facility = new Facility
            {
                Name = name
            };

            await _unitOfWork.Repository<Facility>().AddAsync(facility);
            await _unitOfWork.SaveChangesAsync();

            return facility.Id;
        }
    }
}
