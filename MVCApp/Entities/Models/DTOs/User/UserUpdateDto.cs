using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs.User
{
    public class UserUpdateDto : EntityBaseDto
    {
        [Required(ErrorMessage = "Security stamp is required.")]
        public string SecurityStamp { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
    }
}
