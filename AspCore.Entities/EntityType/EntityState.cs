using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.EntityType
{
    public enum CoreEntityState
    {
        Unchanged,
        Added,
        Modified,
        Deleted,
        Detached
    }
}
