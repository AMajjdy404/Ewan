using Ewan.Application.Helpers;
using Ewan.Application.Helpers;
using Ewan.Application.Helpers;
using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Enums;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdatePropertyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdatePropertyCommand command, CancellationToken cancellationToken)
        {
            var spec = new PropertyWithDetailsSpecification(command.Request.Id);
            var property = await _unitOfWork.Repository<Property>().GetEntityWithSpec(spec);

            if (property == null)
                throw new KeyNotFoundException("Property not found.");

            var propertyType = command.Request.PropertyType;
            var isHall = propertyType == PropertyType.Hall;
            var bookingMode = PropertyBookingModeResolver.ResolveFromPropertyType(propertyType);

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

            // احذف الصور القديمة اللي مش موجودة في ExistingImageUrls
            var imagesToDelete = property.Images
                .Where(img => !command.Request.ExistingImageUrls.Contains(img.ImageUrl))
                .ToList();

            foreach (var img in imagesToDelete)
            {
                DocumentSettings.DeleteFile(img.ImageUrl, "Properties");
                property.Images.Remove(img);
            }

            // ارفع الصور الجديدة
            foreach (var file in command.Request.NewImages)
            {
                var url = await DocumentSettings.UploadFileAsync(file, "Properties");
                property.Images.Add(new PropertyImage { ImageUrl = url });
            }

            property.Name = command.Request.Name.Trim();
            property.Description = command.Request.Description.Trim();
            property.PropertyType = propertyType;
            property.BookingMode = bookingMode;
            property.IsAvailable = command.Request.IsAvailable;
            property.Address = command.Request.Address.Trim();
            property.Location = command.Request.Location.Trim();
            property.PricePerNight = isHall ? 0 : command.Request.PricePerNight ?? 0;
            property.PricePerHour = isHall ? command.Request.PricePerHour ?? 0 : 0;
            property.RoomCount = isHall ? 0 : command.Request.RoomCount ?? 0;
            property.GuestCount = command.Request.GuestCount;

            property.PropertyFacilities.Clear();
            foreach (var fId in requestedFacilityIds)
                property.PropertyFacilities.Add(new PropertyFacility { FacilityId = fId });

            _unitOfWork.Repository<Property>().Update(property);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
