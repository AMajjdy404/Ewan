using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.PropertyGroup;
using MediatR;

namespace Ewan.Application.Features.Dashboard.PropertyGroups.Queries.GetPropertyGroupById
{
   
    public class GetPropertyGroupByIdQueryHandler : IRequestHandler<GetPropertyGroupByIdQuery, PropertyGroupDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPropertyGroupByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PropertyGroupDto> Handle(GetPropertyGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<PropertyGroup>().GetByIdAsync(request.Id);

            if (entity == null)
                throw new KeyNotFoundException("Property group not found.");

            return new PropertyGroupDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
