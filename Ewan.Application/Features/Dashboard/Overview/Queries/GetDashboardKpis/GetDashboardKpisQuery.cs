using Ewan.Core.Models.Dtos.Dashboard;
using MediatR;

namespace Ewan.Application.Features.Dashboard.Overview.Queries.GetDashboardKpis
{
    public record GetDashboardKpisQuery : IRequest<DashboardKpisDto>;
}
