using Ewan.Core.Models.Dtos.Dashboard;
using Ewan.Core.Models.Enums;
using Ewan.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ewan.Application.Features.Dashboard.Overview.Queries.GetDashboardKpis
{
    public class GetDashboardKpisQueryHandler : IRequestHandler<GetDashboardKpisQuery, DashboardKpisDto>
    {
        private readonly AppDbContext _dbContext;

        public GetDashboardKpisQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DashboardKpisDto> Handle(GetDashboardKpisQuery request, CancellationToken cancellationToken)
        {
            var totalProperties = await _dbContext.Properties.CountAsync(cancellationToken);
            var totalClients = await _dbContext.Clients.CountAsync(cancellationToken);
            var totalBookings = await _dbContext.Bookings.CountAsync(cancellationToken);
            var totalRevenue = await _dbContext.Bookings
                .Where(x => x.Status != BookingStatus.Cancelled)
                .SumAsync(x => (decimal?)x.TotalAmount, cancellationToken) ?? 0;

            return new DashboardKpisDto
            {
                TotalProperties = totalProperties,
                TotalClients = totalClients,
                TotalBookings = totalBookings,
                TotalRevenue = totalRevenue
            };
        }
    }
}
