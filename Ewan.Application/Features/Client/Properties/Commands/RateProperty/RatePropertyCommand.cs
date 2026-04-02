using Ewan.Core.Models.Dtos.Property;
using MediatR;

namespace Ewan.Application.Features.Client.Properties.Commands.RateProperty
{
    public record RatePropertyCommand(int ClientId, int PropertyId, RatePropertyRequestDto Request) : IRequest;
}
