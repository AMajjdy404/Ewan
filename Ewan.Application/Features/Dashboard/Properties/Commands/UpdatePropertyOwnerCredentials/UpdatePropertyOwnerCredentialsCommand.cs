using Ewan.Core.Models.Dtos.Property;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.UpdatePropertyOwnerCredentials
{
    public record UpdatePropertyOwnerCredentialsCommand(int PropertyId, UpdatePropertyOwnerCredentialsRequestDto Request) : IRequest;
}
