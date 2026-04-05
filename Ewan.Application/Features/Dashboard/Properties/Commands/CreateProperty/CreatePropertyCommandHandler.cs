using Ewan.Application.Helpers;
using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using BCrypt.Net;

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
            var ownerPhoneNumber = command.Request.OwnerPhoneNumber.Trim();

            var groupExists = await _unitOfWork.Repository<PropertyGroup>()
                .AnyAsync(g => g.Id == command.Request.GroupId);
            if (!groupExists)
                throw new KeyNotFoundException("Property group not found.");

            var ownerPhoneExists = await _unitOfWork.Repository<Property>()
                .AnyAsync(p => p.OwnerPhoneNumber == ownerPhoneNumber);
            if (ownerPhoneExists)
                throw new BadHttpRequestException("Owner phone number already assigned to another property.");

            var requestedFacilityIds = command.Request.FacilityIds
                .Distinct()
                .ToList();

            if (requestedFacilityIds.Count > 0)
            {
                var existingFacilityIds = (await _unitOfWork.Repository<Facility>().ListAllAsync())
                    .Select(x => x.Id)
                    .ToHashSet();

                var invalidFacilityIds = requestedFacilityIds
                    .Where(x => !existingFacilityIds.Contains(x))
                    .ToList();

                if (invalidFacilityIds.Count > 0)
                    throw new BadHttpRequestException($"Invalid facility ids: {string.Join(", ", invalidFacilityIds)}");
            }

            var imageUrls = new List<string>();
            foreach (var file in command.Request.Images)
            {
                var url = await DocumentSettings.UploadFileAsync(file, "Properties");
                imageUrls.Add(url);
            }

            var property = new Property
            {
                Name = command.Request.Name.Trim(),
                Description = command.Request.Description.Trim(),
                OwnerPhoneNumber = ownerPhoneNumber,
                OwnerPasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Request.OwnerPassword.Trim()),
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
                PropertyFacilities = requestedFacilityIds
                    .Select(fId => new PropertyFacility { FacilityId = fId })
                    .ToList()
            };

            await _unitOfWork.Repository<Property>().AddAsync(property);
            await _unitOfWork.SaveChangesAsync();
            return property.Id;
        }
    }
}
