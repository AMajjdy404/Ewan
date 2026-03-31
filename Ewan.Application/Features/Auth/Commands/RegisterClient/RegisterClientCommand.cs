using Ewan.Core.Models.Dtos;
using MediatR;
namespace Ewan.Application.Features.Auth.Commands.RegisterClient
{
    

    namespace Ewan.Application.Features.Auth.Commands.RegisterClient
    {
        public record RegisterClientCommand(
            RegisterClientDto Request,
            string IpAddress
        ) : IRequest<AuthResponseDto>;
    }
}
