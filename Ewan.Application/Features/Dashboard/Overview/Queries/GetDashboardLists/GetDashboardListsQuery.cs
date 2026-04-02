using Ewan.Core.Models.Dtos.Dashboard;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Overview.Queries.GetDashboardLists
{
    public record GetDashboardListsQuery(int TopCount = 5, int RecentCount = 5) : IRequest<DashboardListsDto>;
}
