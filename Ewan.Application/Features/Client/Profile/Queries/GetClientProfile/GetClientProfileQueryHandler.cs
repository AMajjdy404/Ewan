using Ewan.Core.Interfaces;
using Ewan.Core.Models.Dtos.Client;
using MediatR;

namespace Ewan.Application.Features.Client.Profile.Queries.GetClientProfile
{
    public class GetClientProfileQueryHandler : IRequestHandler<GetClientProfileQuery, ClientProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetClientProfileQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ClientProfileDto> Handle(GetClientProfileQuery request, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.Repository<global::Ewan.Core.Models.Client>().GetByIdAsync(request.ClientId);
            if (client == null || client.IsDeleted)
                throw new KeyNotFoundException("Client not found.");

            return new ClientProfileDto
            {
                Id = client.Id,
                FullName = client.FullName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                CreatedAt = client.CreatedAt
            };
        }
    }
}
