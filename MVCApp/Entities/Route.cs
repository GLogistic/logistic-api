using Entities.Base;

namespace Entities
{
    public class Route : EntityBase
    {
        public int Distance { get; set; }

        public Guid StartSettlementId { get; set; }
        public Settlement StartSettlement { get; set; }

        public Guid EndSettlementId { get; set; }
        public Settlement EndSettlement { get; set; }
    }
}
