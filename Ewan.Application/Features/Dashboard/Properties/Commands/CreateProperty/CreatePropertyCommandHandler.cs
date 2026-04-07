using Ewan.Application.Helpers;
using Ewan.Application.Helpers;
using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Enums;
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
            var propertyType = command.Request.PropertyType;
            var isHall = propertyType == PropertyType.Hall;

            var bookingMode = PropertyBookingModeResolver.ResolveFromPropertyType(propertyType);

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
                PropertyType = propertyType,
                BookingMode = bookingMode,
                Address = command.Request.Address.Trim(),
                Location = command.Request.Location.Trim(),
                PricePerNight = isHall ? 0 : command.Request.PricePerNight ?? 0,
                PricePerHour = isHall ? command.Request.PricePerHour ?? 0 : 0,
                RoomCount = isHall ? 0 : command.Request.RoomCount ?? 0,
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
