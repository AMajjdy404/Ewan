using Ewan.Core.Models.Dtos.Dashboard;
using Ewan.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ewan.Application.Features.Dashboard.Overview.Queries.GetDashboardCharts
{
    public class GetDashboardChartsQueryHandler : IRequestHandler<GetDashboardChartsQuery, DashboardChartsDto>
    {
        private readonly AppDbContext _dbContext;

        public GetDashboardChartsQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DashboardChartsDto> Handle(GetDashboardChartsQuery request, CancellationToken cancellationToken)
        {
            var monthsCount = request.Months <= 0 ? 6 : request.Months;
            var startMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(-(monthsCount - 1));

            var bookingMonthlyRaw = await _dbContext.Bookings
                .Where(x => x.CreatedAt >= startMonth)
                .GroupBy(x => new { x.CreatedAt.Year, x.CreatedAt.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    BookingsCount = g.Count(),
                    Revenue = g.Sum(x => x.TotalAmount)
                })
                .ToListAsync(cancellationToken);

            var revenueTrend = new List<DashboardMonthlyPointDto>();
            var bookingsTrend = new List<DashboardMonthlyPointDto>();

            for (var i = 0; i < monthsCount; i++)
            {
                var monthDate = startMonth.AddMonths(i);
                var monthRaw = bookingMonthlyRaw.FirstOrDefault(x => x.Year == monthDate.Year && x.Month == monthDate.Month);

                revenueTrend.Add(new DashboardMonthlyPointDto
                {
                    Month = monthDate.ToString("MMM yyyy"),
                    Value = monthRaw?.Revenue ?? 0
                });

                bookingsTrend.Add(new DashboardMonthlyPointDto
                {
                    Month = monthDate.ToString("MMM yyyy"),
                    Value = monthRaw?.BookingsCount ?? 0
                });
            }

            var bookingsByCategoryRaw = await _dbContext.Bookings
                .GroupBy(x => x.Property.PropertyType)
                .Select(g => new { PropertyType = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync(cancellationToken);

            var totalCategoryBookings = bookingsByCategoryRaw.Sum(x => x.Count);
            var bookingsByCategory = bookingsByCategoryRaw.Select(x => new DashboardDistributionItemDto
            {
                Name = x.PropertyType switch
                {
                    Core.Models.Enums.PropertyType.Chalet => "ÔÇáíÉ",
                    Core.Models.Enums.PropertyType.Hotel => "ÝäÏÞ",
                    Core.Models.Enums.PropertyType.Apartment => "ÔÞÉ",
                    Core.Models.Enums.PropertyType.Hall => "ÞÇÚÉ",
                    _ => x.PropertyType.ToString()
                },
                Count = x.Count,
                Percentage = totalCategoryBookings == 0 ? 0 : Math.Round((decimal)x.Count * 100 / totalCategoryBookings, 2)
            }).ToList();

            var bookingsByCityRaw = await _dbContext.Bookings
                .GroupBy(x => x.Property.Location)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync(cancellationToken);

            var totalCityBookings = bookingsByCityRaw.Sum(x => x.Count);
            var bookingsByCity = bookingsByCityRaw.Select(x => new DashboardDistributionItemDto
            {
                Name = x.Name,
                Count = x.Count,
                Percentage = totalCityBookings == 0 ? 0 : Math.Round((decimal)x.Count * 100 / totalCityBookings, 2)
            }).ToList();

            return new DashboardChartsDto
            {
                RevenueTrend = revenueTrend,
                BookingsTrend = bookingsTrend,
                BookingsByCategory = bookingsByCategory,
                BookingsByCity = bookingsByCity
            };
        }
    }
}
