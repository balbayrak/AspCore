using System;
using System.Collections.Generic;
using AspCore.Entities.EntityType;

namespace AspCoreTest.Entities.Models
{
    public partial class Role : BaseEntity
    {
        public Role()
        {
            PersonRole = new HashSet<PersonRole>();
        }

        public Guid Name { get; set; }

        public virtual ICollection<PersonRole> PersonRole { get; set; }
    }
}
