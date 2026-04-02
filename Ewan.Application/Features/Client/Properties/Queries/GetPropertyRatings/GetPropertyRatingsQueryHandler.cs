using Ewan.Core.Interfaces;
using Ewan.Core.Models;
using Ewan.Core.Models.Dtos.Property;
using Ewan.Infrastructure.ReposAndSpecs;
using MediatR;

namespace Ewan.Application.Features.Client.Properties.Queries.GetPropertyRatings
{
    public class GetPropertyRatingsQueryHandler : IRequestHandler<GetPropertyRatingsQuery, PropertyRatingsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPropertyRatingsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PropertyRatingsDto> Handle(GetPropertyRatingsQuery request, CancellationToken cancellationToken)
        {
            var spec = new PropertyWithRatingsSpecification(request.PropertyId);
            var property = await _unitOfWork.Repository<Property>().GetEntityWithSpec(spec);

            if (property == null)
                throw new KeyNotFoundException("Property not found.");

            var ratings = property.Ratings
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new PropertyRatingItemDto
                {
                    ClientId = x.ClientId,
                    ClientName = x.Client.FullName,
                    Rate = x.Rate,
                    Comment = x.Comment,
                    CreatedAt = x.CreatedAt
                })
                .ToList();

            return new PropertyRatingsDto
            {
                PropertyId = property.Id,
                RatingsCount = ratings.Count,
                AverageRate = ratings.Count > 0 ? Math.Round(ratings.Average(x => x.Rate), 1) : 0,
                Ratings = ratings
            };
        }
    }
}
