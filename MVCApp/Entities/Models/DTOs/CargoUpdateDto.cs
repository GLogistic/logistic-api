using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class CargoUpdateDto : EntityBaseDto
    {
        [Required(ErrorMessage = "Cargo Title is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Cargo Weight  is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Cargo Weight must be a positive number.")]
        public int Weight { get; set; }
        [Required(ErrorMessage = "Cargo Registration number is required.")]
        public string RegistrationNumber { get; set; }
    }
}
