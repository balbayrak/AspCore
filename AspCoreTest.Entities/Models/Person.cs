using System.Collections.Generic;
using AspCore.Entities.EntityType;

namespace AspCoreTest.Entities.Models
{
    public partial class Person : BaseEntity
    {
        public Person()
        {
            PersonAddress = new HashSet<PersonAddress>();
            PersonCv = new HashSet<PersonCv>();
            PersonRole = new HashSet<PersonRole>();
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual Admin Admin { get; set; }

        public virtual ICollection<PersonAddress> PersonAddress { get; set; }
        public virtual ICollection<PersonCv> PersonCv { get; set; }
        public virtual ICollection<PersonRole> PersonRole { get; set; }
    }
}
