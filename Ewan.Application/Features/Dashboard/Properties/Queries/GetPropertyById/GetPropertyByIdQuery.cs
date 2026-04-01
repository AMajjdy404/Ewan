using Ewan.Core.Models.Dtos.Property;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Properties.Queries.GetPropertyById
{
    public record GetPropertyByIdQuery(int Id) : IRequest<PropertyDto>;

}
