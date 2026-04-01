using Ewan.Application.Helpers;
using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreatePropertyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreatePropertyCommand command, CancellationToken cancellationToken)
        {
            var groupExists = await _unitOfWork.Repository<PropertyGroup>()
                .AnyAsync(g => g.Id == command.Request.GroupId);
            if (!groupExists)
                throw new KeyNotFoundException("Property group not found.");

            var imageUrls = new List<string>();
            foreach (var file in command.Request.Images)
            {
                var url = await DocumentSettings.UploadFileAsync(file, "Properties");
                imageUrls.Add(url);
            }

            var property = new Property
            {
                Name = command.Request.Name.Trim(),
                GroupId = command.Request.GroupId,
                Address = command.Request.Address.Trim(),
                Location = command.Request.Location.Trim(),
                PricePerNight = command.Request.PricePerNight,
                RoomCount = command.Request.RoomCount,
                GuestCount = command.Request.GuestCount,
                IsAvailable = true,
                Images = imageUrls
                    .Select(url => new PropertyImage { ImageUrl = url })
                    .ToList(),
                PropertyFacilities = command.Request.FacilityIds
                    .Select(fId => new PropertyFacility { FacilityId = fId })
                    .ToList()
            };

            await _unitOfWork.Repository<Property>().AddAsync(property);
            await _unitOfWork.SaveChangesAsync();
            return property.Id;
        }
    }
}
