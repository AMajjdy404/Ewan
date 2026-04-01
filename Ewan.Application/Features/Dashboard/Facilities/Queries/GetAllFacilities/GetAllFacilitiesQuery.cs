using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Facility;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Facilities.Queries.GetAllFacilities
{
    public record GetAllFacilitiesQuery(PaginationParams Params) : IRequest<Pagination<FacilityDto>>;
}
