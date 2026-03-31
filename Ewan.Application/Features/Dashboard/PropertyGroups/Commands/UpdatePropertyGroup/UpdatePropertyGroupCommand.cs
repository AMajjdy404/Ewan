using Ewan.Core.Models.Dtos.PropertyGroup;
using MediatR;

namespace Ewan.Application.Features.Dashboard.PropertyGroups.Commands.UpdatePropertyGroup
{
    public record UpdatePropertyGroupCommand(UpdatePropertyGroupRequestDto Request) : IRequest<bool>;
}
