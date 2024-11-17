using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs.User
{
    public class UserDeleteDto
    {
        [Required(ErrorMessage = "Settlement Id is required.")]
        public Guid Id { get; set; }
    }
}
