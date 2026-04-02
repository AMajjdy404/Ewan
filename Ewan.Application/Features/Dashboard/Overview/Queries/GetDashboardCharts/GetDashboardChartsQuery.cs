using Ewan.Core.Models.Dtos.Dashboard;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Overview.Queries.GetDashboardCharts
{
    public record GetDashboardChartsQuery(int Months = 6) : IRequest<DashboardChartsDto>;
}
