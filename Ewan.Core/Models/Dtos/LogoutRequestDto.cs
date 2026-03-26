using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ewan.Core.Models.Dtos
{
    public class LogoutRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
