using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class SettlementDeleteDto
    {
        [Required(ErrorMessage = "Settlement Id is required.")]
        public Guid Id { get; set; }
    }
}
