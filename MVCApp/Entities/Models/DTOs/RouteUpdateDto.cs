using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class RouteUpdateDto : EntityBaseDto
    {
        [Required(ErrorMessage = "Route Start settlement is required.")]
        public Guid StartSettlementId { get; set; }
        [Required(ErrorMessage = "Route End settlement is required.")]
        public Guid EndSettlementId { get; set; }
        [Required(ErrorMessage = "Route Distance is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Route Distance must be a positive number.")]
        public int Distance { get; set; }

        public ValidationResult ValidateSettlements()
        {
            if (StartSettlementId == EndSettlementId)
                return new ValidationResult("Start and End settlements must be different.");

            return ValidationResult.Success!;
        }
    }
}
