using Entities.Base;

namespace Entities.Models.DTOs.User
{
    public class UserDto : EntityBaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
