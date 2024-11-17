using Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class User : IdentityUser<Guid>, IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
