using Entities.Base;

namespace Entities
{
    public class Settlement : EntityBase
    {
        public string Title { get; set; }

        public IEnumerable<Route>? RouteStartSettlements { get; set; }

        public IEnumerable<Route>? RouteEndSettlements { get; set; }
    }
}
