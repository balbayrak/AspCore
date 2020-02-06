using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.EntityType
{
    public interface IBaseEntity : IEntity
    {
        DateTime CreatedDate { get; set; }
        DateTime LastUpdateDate { get; set; }
        bool IsDeleted { get; set; }
        Guid LastUpdatedUserId { get; set; }
    }
}
