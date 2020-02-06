using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.EntityType
{
    public interface IJWTEntity
    {
        string correlationId { get; set; }

        Guid activeUserId { get; set; }
    }
}
