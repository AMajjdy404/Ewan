using Ewan.Core.Models.Dtos.Property;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.UpdateProperty
{
    public record UpdatePropertyCommand(UpdatePropertyRequestDto Request) : IRequest<bool>;

}
