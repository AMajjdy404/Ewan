using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.UpdatePropertyOwnerCredentials
{
    public class UpdatePropertyOwnerCredentialsCommandHandler : IRequestHandler<UpdatePropertyOwnerCredentialsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePropertyOwnerCredentialsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdatePropertyOwnerCredentialsCommand request, CancellationToken cancellationToken)
        {
            var property = await _unitOfWork.Repository<Property>().GetByIdAsync(request.PropertyId);
            if (property == null)
                throw new KeyNotFoundException("Property not found.");

            var ownerPhoneNumber = request.Request.OwnerPhoneNumber.Trim();

            var ownerPhoneExists = await _unitOfWork.Repository<Property>()
                .AnyAsync(p => p.OwnerPhoneNumber == ownerPhoneNumber && p.Id != property.Id);
            if (ownerPhoneExists)
                throw new BadHttpRequestException("Owner phone number already assigned to another property.");

            property.OwnerPhoneNumber = ownerPhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.Request.OwnerPassword))
                property.OwnerPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Request.OwnerPassword.Trim());

            _unitOfWork.Repository<Property>().Update(property);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
