using Ewan.Core.Models.Dtos.TermsAndConditions;
using MediatR;

namespace Ewan.Application.Features.Dashboard.TermsAndConditions.Queries.GetTermsAndConditions
{
    public record GetTermsAndConditionsQuery : IRequest<TermsAndConditionsDto>;
}
