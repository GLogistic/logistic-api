using Entities.Base;

namespace Entities
{
    public class Cargo : EntityBase
    {
        public string Title { get; set; }
        public int Weight { get; set; }
        public string RegistrationNumber { get; set; }
    }
}
