using System;

namespace AspCore.AOP.Concrete
{
    public class InterceptorType
    {
        public Type type { get; set; }

        public int priority { get; set; }

        public EnumInterceptorRunType runType { get; set; }
    }
}
