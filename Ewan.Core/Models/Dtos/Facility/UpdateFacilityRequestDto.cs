using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ewan.Core.Models.Dtos.Facility
{
    public class UpdateFacilityRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
