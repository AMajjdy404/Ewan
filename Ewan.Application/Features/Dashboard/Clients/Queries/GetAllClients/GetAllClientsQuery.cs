using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Client;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Clients.Queries.GetAllClients
{
    public record GetAllClientsQuery(PaginationParams Params) : IRequest<Pagination<DashboardClientDto>>;
}
