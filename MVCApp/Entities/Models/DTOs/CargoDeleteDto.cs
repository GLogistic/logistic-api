using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class CargoDeleteDto
    {
        [Required(ErrorMessage = "Cargo Id is required.")]
        public Guid Id { get; set; }
    }
}
