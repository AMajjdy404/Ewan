using Ewan.Core.Models;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class RefreshTokenByHashSpecification : BaseSpecification<RefreshToken>
    {
        public RefreshTokenByHashSpecification(string tokenHash)
            : base(x => x.TokenHash == tokenHash)
        {
        }
    }
}
