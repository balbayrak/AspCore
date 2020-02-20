using AspCore.Entities.EntityType;

namespace AspCoreTest.Entities.Models
{
    public partial class Admin : CoreEntity
    {
        public string Description { get; set; }
        public virtual Person Person { get; set; }
    }
}
