using Ewan.Core.Models.Dtos.Facility;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Facilities.Queries.GetFacilityById
{
    public record GetFacilityByIdQuery(int Id) : IRequest<FacilityDto>;
}
