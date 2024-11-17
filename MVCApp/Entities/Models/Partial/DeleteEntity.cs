using Entities.Models.Partial.Helpers;

namespace Entities.Models.Partial
{
    public class DeleteEntity
    {
        public ControllerInfo ControllerInfo { get; set; }
        public Guid? Id { get; set; }
        public string PopUpId { get; set; }
        public string ItemName { get; set; }
    }
}
