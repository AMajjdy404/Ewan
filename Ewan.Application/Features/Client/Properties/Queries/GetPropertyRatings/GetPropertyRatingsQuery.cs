using Ewan.Core.Models.Dtos.Property;
using MediatR;

namespace Ewan.Application.Features.Client.Properties.Queries.GetPropertyRatings
{
    public record GetPropertyRatingsQuery(int PropertyId) : IRequest<PropertyRatingsDto>;
}
