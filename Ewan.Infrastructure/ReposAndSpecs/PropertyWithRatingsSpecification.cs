using Ewan.Core.Models;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class PropertyWithRatingsSpecification : BaseSpecification<Property>
    {
        public PropertyWithRatingsSpecification(int propertyId)
            : base(x => x.Id == propertyId)
        {
            AddInclude("Ratings.Client");
        }
    }
}
