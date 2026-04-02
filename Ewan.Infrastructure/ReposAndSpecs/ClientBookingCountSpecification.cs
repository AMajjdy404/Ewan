using Ewan.Core.Models;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class ClientBookingCountSpecification : BaseSpecification<Booking>
    {
        public ClientBookingCountSpecification(int clientId)
            : base(x => x.ClientId == clientId)
        {
        }
    }
}
