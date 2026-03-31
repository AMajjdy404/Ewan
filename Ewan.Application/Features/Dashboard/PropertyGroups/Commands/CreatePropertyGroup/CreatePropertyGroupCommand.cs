using Ewan.Core.Models.Dtos.PropertyGroup;
using MediatR;

namespace Ewan.Application.Features.Dashboard.PropertyGroups.Commands.CreatePropertyGroup
{
    public record CreatePropertyGroupCommand(CreatePropertyGroupRequestDto Request) : IRequest<int>;
}
