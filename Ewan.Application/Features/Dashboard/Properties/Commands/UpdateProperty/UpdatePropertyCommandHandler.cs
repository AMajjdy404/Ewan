using Ewan.Application.Helpers;
using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;

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

            var groupExists = await _unitOfWork.Repository<PropertyGroup>()
                .AnyAsync(g => g.Id == command.Request.GroupId);
            if (!groupExists)
                throw new KeyNotFoundException("Property group not found.");

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
            property.GroupId = command.Request.GroupId;
            property.IsAvailable = command.Request.IsAvailable;
            property.Address = command.Request.Address.Trim();
            property.Location = command.Request.Location.Trim();
            property.PricePerNight = command.Request.PricePerNight;
            property.RoomCount = command.Request.RoomCount;
            property.GuestCount = command.Request.GuestCount;

            property.PropertyFacilities.Clear();
            foreach (var fId in command.Request.FacilityIds)
                property.PropertyFacilities.Add(new PropertyFacility { FacilityId = fId });

            _unitOfWork.Repository<Property>().Update(property);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
