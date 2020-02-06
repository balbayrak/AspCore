using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Dependency.Abstract;

namespace AspCore.AOP.Abstract
{
    public interface IInterceptorContext : IScopedType
    {
        IInvocation invocation { get; set; }
    }
}
