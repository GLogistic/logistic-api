using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class RouteDeleteDto
    {
        [Required(ErrorMessage = "Route Id is required.")]
        public Guid Id { get; set; }
    }
}
