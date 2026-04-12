using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.ContactUs;
using MediatR;

namespace Ewan.Application.Features.Dashboard.ContactUs.Queries.GetContactUs
{
    public class GetContactUsQueryHandler : IRequestHandler<GetContactUsQuery, ContactUsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetContactUsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ContactUsDto> Handle(GetContactUsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<ContactUsSetting>().FirstOrDefaultAsync(x => true);
            if (entity == null)
                return new ContactUsDto();

            return new ContactUsDto
            {
                SupportNumber = entity.SupportNumber,
                WhatsappNumber = entity.WhatsappNumber,
                Email = entity.Email
            };
        }
    }
}
