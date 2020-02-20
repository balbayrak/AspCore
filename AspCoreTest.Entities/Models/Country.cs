using System.Collections.Generic;
using AspCore.Entities.EntityType;

namespace AspCoreTest.Entities.Models
{
    public partial class Country : BaseEntity
    {
        public Country()
        {
            City = new HashSet<City>();
        }

        public string Name { get; set; }
        public virtual ICollection<City> City { get; set; }
    }
}
