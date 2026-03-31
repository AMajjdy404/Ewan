using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ewan.Core.Models
{
    public class Facility
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<PropertyFacility> PropertyFacilities { get; set; } = new HashSet<PropertyFacility>();
    }
}
