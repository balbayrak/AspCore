using System;
using AspCore.Entities.EntityType;

namespace AspCoreTest.Entities.Models
{
    public partial class PersonRole : CoreEntity
    {
        public Guid RoleId { get; set; }
        public Guid PersonId { get; set; }

        public virtual Person Person { get; set; }
        public virtual Role Role { get; set; }
    }
}
