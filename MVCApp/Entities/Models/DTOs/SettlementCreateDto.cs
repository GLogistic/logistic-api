using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class SettlementCreateDto
    {
        [Required(ErrorMessage = "Settlement Title is required.")]
        public string Title { get; set; }
    }
}
