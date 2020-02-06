using System;
using System.Collections.Generic;
using System.Text;
using AspCore.AOP.Abstract;

namespace AspCore.AOP.Concrete
{
    public class InterceptorContext : IInterceptorContext
    {
        public IInvocation invocation { get; set; }
    }
}
