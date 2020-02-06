using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.AOP.Concrete
{
    public enum EnumInterceptorRunType
    {
        Before = 1,
        After = 2,
        Exception = 3,
        BeforeAfter = 4
    }
}
