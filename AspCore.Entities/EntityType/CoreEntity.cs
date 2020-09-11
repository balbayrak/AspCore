using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AspCore.Entities.EntityType
{
    public abstract class CoreEntity : IEntity
    {
        [NotMapped]
        public CoreEntityState? entityState { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        public CoreEntity()
        {
        }
    }
}
