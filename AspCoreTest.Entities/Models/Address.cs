using System;
using System.Collections.Generic;
using AspCore.Entities.EntityType;

namespace AspCoreTest.Entities.Models
{
    public class Address : BaseEntity
    {
        public Address()
        {
            PersonAddress = new HashSet<PersonAddress>();
        }

        public string FullAddress { get; set; }
        public Guid CityId { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<PersonAddress> PersonAddress { get; set; }
    }
}
