using Ewan.Core.Models.Dtos.TermsAndConditions;
using MediatR;

namespace Ewan.Application.Features.Dashboard.TermsAndConditions.Commands.UpsertTermsAndConditions
{
    public record UpsertTermsAndConditionsCommand(UpdateTermsAndConditionsRequestDto Request) : IRequest;
}
