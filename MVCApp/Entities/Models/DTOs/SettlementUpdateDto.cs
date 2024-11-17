using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class SettlementUpdateDto : EntityBaseDto
    {
        [Required(ErrorMessage = "Settlement Title is required.")]
        public string Title { get; set; }
    }
}
