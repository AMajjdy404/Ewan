using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ewan.Application.Features.Dashboard.PropertyGroups.Commands.CreatePropertyGroup
{
    

    public class CreatePropertyGroupCommandHandler : IRequestHandler<CreatePropertyGroupCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePropertyGroupCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreatePropertyGroupCommand command, CancellationToken cancellationToken)
        {
            var name = command.Request.Name.Trim();

            var exists = await _unitOfWork.Repository<PropertyGroup>()
                .AnyAsync(x => x.Name == name);

            if (exists)
                throw new BadHttpRequestException("Property group name already exists.");

            var entity = new PropertyGroup
            {
                Name = name
            };

            await _unitOfWork.Repository<PropertyGroup>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }
    }
}
