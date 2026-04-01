using MediatR;

namespace Ewan.Application.Features.Dashboard.Properties.Commands.DeleteProperty
{
    public record DeletePropertyCommand(int Id) : IRequest<bool>;

}
