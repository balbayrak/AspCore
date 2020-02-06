using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.AOP.Concrete
{
    public class InterceptorType
    {
        public Type type { get; set; }

        public int priority { get; set; }

        public EnumInterceptorRunType runType { get; set; }
    }
}
