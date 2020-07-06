using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.EntityType
{
    public interface IJWTEntity
    {
        Guid authenticatedUserId { get; set; }
    }
}
