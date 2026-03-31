using MediatR;

namespace Ewan.Application.Features.Dashboard.PropertyGroups.Commands.DeletePropertyGroup
{
    public record DeletePropertyGroupCommand(int Id) : IRequest<bool>;
}
