using Ewan.Core.Models;
using Ewan.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class ExpiredNonRevokedTokensSpecification : BaseSpecification<RefreshToken>
    {
        public ExpiredNonRevokedTokensSpecification(int retainDays = 7)
        {
            var threshold = DateTime.UtcNow.AddDays(-retainDays);

            Criteria = rt =>
                rt.ExpiresAt < threshold &&
                !rt.IsRevoked;
        }
    }
}
