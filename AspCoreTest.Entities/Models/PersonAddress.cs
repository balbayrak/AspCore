using System;
using AspCore.Entities.EntityType;

namespace AspCoreTest.Entities.Models
{
    public partial class PersonAddress : CoreEntity
    {
        public Guid PersonId { get; set; }
        public Guid AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Person Person { get; set; }
    }
}
