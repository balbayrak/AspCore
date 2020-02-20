using System;
using AspCore.Entities.EntityType;

namespace AspCoreTest.Entities.Models
{
    public partial class PersonCv : BaseEntity, IDocumentEntity
    {
        public string Name { get; set; }
        public string DocumentUrl { get; set; }
        public Guid PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
