using Ewan.Core.Models;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class OldRevokedTokensSpecification : BaseSpecification<RefreshToken>
    {
        public OldRevokedTokensSpecification(int retainDays = 90)
        {
            var threshold = DateTime.UtcNow.AddDays(-retainDays);

            Criteria = rt =>
                rt.IsRevoked &&
                rt.RevokedAt.HasValue &&
                rt.RevokedAt.Value < threshold;
        }
    }
}
