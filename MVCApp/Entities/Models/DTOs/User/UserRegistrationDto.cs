using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs.User
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Password must be at least 10 characters long.")]
        public string Password { get; set; }
    }
}
