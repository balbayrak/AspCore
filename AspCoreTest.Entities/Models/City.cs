using System;
using System.Collections.Generic;
using AspCore.Entities.EntityType;

namespace AspCoreTest.Entities.Models
{
    public partial class City : BaseEntity
    {
        public City()
        {
            Address = new HashSet<Address>();
        }

        public string Name { get; set; }
        public Guid CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Address> Address { get; set; }
    }
}
