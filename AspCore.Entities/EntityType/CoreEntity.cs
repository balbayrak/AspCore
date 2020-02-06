using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AspCore.Entities.EntityType
{
    public class CoreEntity : IEntity
    {
        [NotMapped]
        public CoreEntityState? entityState { get; set; }
        public Guid Id { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public CoreEntity()
        {
            this.Id = Guid.NewGuid();
            this.entityState = CoreEntityState.Added;
        }
    }
}
