using Ewan.Core.Models;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class OtherDeviceRefreshTokensSpecification : BaseSpecification<RefreshToken>
    {
        public OtherDeviceRefreshTokensSpecification(string ownerId, string userType, string currentDeviceId)
            : base(x =>
                x.OwnerId == ownerId &&
                x.UserType == userType &&
                x.DeviceId != currentDeviceId &&
                !x.IsRevoked &&
                !x.IsCompromised &&
                x.ExpiresAt > DateTime.UtcNow)
        {
        }
    }
}
