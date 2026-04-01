using Ewan.Core.Models.Dtos.Facility;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Facilities.Commands.UpdateFacility
{
    public record UpdateFacilityCommand(UpdateFacilityRequestDto Request) : IRequest<bool>;
}
