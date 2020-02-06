using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.AOP.Abstract
{
    public interface IProxyGenerator
    {
        object Create(Type serviceType, Type implementationType, object implementationObj, IInterceptorContext context);
    }
}
