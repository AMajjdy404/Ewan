using Ewan.Application.Helpers;
using Ewan.Application.Helpers;
using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.TermsAndConditions;
using MediatR;

namespace Ewan.Application.Features.Dashboard.TermsAndConditions.Queries.GetTermsAndConditions
{
    public class GetTermsAndConditionsQueryHandler : IRequestHandler<GetTermsAndConditionsQuery, TermsAndConditionsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTermsAndConditionsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TermsAndConditionsDto> Handle(GetTermsAndConditionsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.Repository<TermsAndConditionsSetting>().FirstOrDefaultAsync(x => true);
            var content = entity?.Content ?? string.Empty;

            return new TermsAndConditionsDto
            {
                Content = content,
                HtmlContent = TextToHtmlConverter.ConvertPlainTextToHtml(content)
            };
        }
    }
}
