using MediatR;

namespace Ewan.Application.Features.Dashboard.Facilities.Commands.DeleteFacility
{
    public record DeleteFacilityCommand(int Id) : IRequest<bool>;
}
