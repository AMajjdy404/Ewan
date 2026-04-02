using Ewan.Core.Models.Dtos.Dashboard;
using Ewan.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ewan.Application.Features.Dashboard.Overview.Queries.GetDashboardLists
{
    public class GetDashboardListsQueryHandler : IRequestHandler<GetDashboardListsQuery, DashboardListsDto>
    {
        private readonly AppDbContext _dbContext;

        public GetDashboardListsQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DashboardListsDto> Handle(GetDashboardListsQuery request, CancellationToken cancellationToken)
        {
            var topCount = request.TopCount <= 0 ? 5 : request.TopCount;
            var recentCount = request.RecentCount <= 0 ? 5 : request.RecentCount;

            var topBookedProperties = await _dbContext.Bookings
                .GroupBy(x => new { x.PropertyId, x.Property.Name })
                .Select(g => new DashboardTopPropertyDto
                {
                    PropertyId = g.Key.PropertyId,
                    PropertyName = g.Key.Name,
                    BookingsCount = g.Count(),
                    Revenue = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(x => x.BookingsCount)
                .ThenByDescending(x => x.Revenue)
                .Take(topCount)
                .ToListAsync(cancellationToken);

            var recentBookings = await _dbContext.Bookings
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new DashboardRecentBookingDto
                {
                    BookingId = x.Id,
                    ClientName = x.Client.FullName,
                    PropertyName = x.Property.Name,
                    TotalAmount = x.TotalAmount,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt
                })
                .Take(recentCount)
                .ToListAsync(cancellationToken);

            return new DashboardListsDto
            {
                TopBookedProperties = topBookedProperties,
                RecentBookings = recentBookings
            };
        }
    }
}
