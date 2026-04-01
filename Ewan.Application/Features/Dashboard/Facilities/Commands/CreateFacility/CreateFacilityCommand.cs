using Ewan.Core.Models.Dtos.Facility;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Facilities.Commands.CreateFacility
{
    public record CreateFacilityCommand(CreateFacilityRequestDto Request) : IRequest<int>;
}
