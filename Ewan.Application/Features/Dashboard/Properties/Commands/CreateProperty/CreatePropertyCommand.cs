using Ewan.Core.Models.Dtos.Property;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.CreateProperty
{
    public record CreatePropertyCommand(CreatePropertyRequestDto Request) : IRequest<int>;

}
