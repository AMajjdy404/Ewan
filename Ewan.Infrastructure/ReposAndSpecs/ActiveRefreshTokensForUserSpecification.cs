using Ewan.Core.Models;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class ActiveRefreshTokensForUserSpecification : BaseSpecification<RefreshToken>
    {
        public ActiveRefreshTokensForUserSpecification(string ownerId, string userType)
            : base(x =>
                x.OwnerId == ownerId &&
                x.UserType == userType &&
                !x.IsRevoked &&
                !x.IsCompromised &&
                x.ExpiresAt > DateTime.UtcNow)
        {
            ApplyOrderBy(x => x.CreatedAt);
        }
    }
}
