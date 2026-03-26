using Ewan.Core.Models;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class ClientPasswordResetTokenByHashSpecification : BaseSpecification<ClientPasswordResetToken>
    {
        public ClientPasswordResetTokenByHashSpecification(int clientId, string tokenHash)
            : base(x =>
                x.ClientId == clientId &&
                x.TokenHash == tokenHash &&
                !x.IsUsed &&
                x.ExpiresAt > DateTime.UtcNow)
        {
        }
    }
}
