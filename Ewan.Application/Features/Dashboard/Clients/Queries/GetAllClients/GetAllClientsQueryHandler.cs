using Ewan.Core.Models.Dtos;
using Ewan.Core.Models.Dtos.Client;
using Ewan.Core.Models.Enums;
using Ewan.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ewan.Application.Features.Dashboard.Clients.Queries.GetAllClients
{
    public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, Pagination<DashboardClientDto>>
    {
        private readonly AppDbContext _dbContext;

        public GetAllClientsQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Pagination<DashboardClientDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            var clientsQuery = _dbContext.Clients
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            var totalCount = await clientsQuery.CountAsync(cancellationToken);

            var clients = await clientsQuery
                .Skip((request.Params.PageIndex - 1) * request.Params.PageSize)
                .Take(request.Params.PageSize)
                .Select(c => new DashboardClientDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    IsActive = c.IsActive,
                    BookingsCount = _dbContext.Bookings.Count(b => b.ClientId == c.Id),
                    TotalBookingsAmount = _dbContext.Bookings
                        .Where(b => b.ClientId == c.Id && b.Status != BookingStatus.Cancelled)
                        .Sum(b => (decimal?)b.TotalAmount) ?? 0,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new Pagination<DashboardClientDto>(
                request.Params.PageIndex,
                request.Params.PageSize,
                totalCount,
                clients);
        }
    }
}
