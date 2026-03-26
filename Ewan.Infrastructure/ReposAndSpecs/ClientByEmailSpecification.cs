using Ewan.Core.Models;
using Ewan.Core.Specifications;

namespace Ewan.Infrastructure.ReposAndSpecs
{
    public class ClientByEmailSpecification : BaseSpecification<Client>
    {
        public ClientByEmailSpecification(string email)
            : base(x => x.Email == email)
        {
        }
    }
}
