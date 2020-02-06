using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.Entities.Expression
{
    public enum EnumSortingDirection
    {
        [Description("asc")]
        Ascending = 1,
        [Description("desc")]
        Descending = 2,
    }
}
