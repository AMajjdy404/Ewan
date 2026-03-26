using Ewan.Core.Models;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class ActiveClientPasswordResetTokensSpecification : BaseSpecification<ClientPasswordResetToken>
    {
        public ActiveClientPasswordResetTokensSpecification(int clientId)
            : base(x =>
                x.ClientId == clientId &&
                !x.IsUsed &&
                x.ExpiresAt > DateTime.UtcNow)
        {
        }
    }
}
