using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ServiceHelpers
{
    public class AuthorizeResult
    {
        public Jwt Token { get; set; }
        public Guid UserId { get; set; }
    }
}
