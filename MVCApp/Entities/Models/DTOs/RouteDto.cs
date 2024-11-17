using Entities.Base;

namespace Entities.Models.DTOs
{
    public class RouteDto : EntityBaseDto
    {
        public SettlementDto? StartSettlement { get; set; }

        public SettlementDto? EndSettlement { get; set; }

        public int Distance { get; set; }
    }
}
