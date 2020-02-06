using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AspCore.Entities.EntityType
{
    public class BaseEntity : CoreEntity, IBaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid LastUpdatedUserId { get; set; }

        public BaseEntity()
        {
            this.CreatedDate = DateTime.Now;
            this.LastUpdateDate = DateTime.Now;
            this.IsDeleted = false;
            LastUpdatedUserId = Guid.Empty;
        }
    }
}
