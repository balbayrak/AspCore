using System;

namespace AspCore.AOP.Abstract
{
    public interface IProxyGenerator
    {
        object Create(Type serviceType, Type implementationType, object implementationObj, IInterceptorContext context);
    }
}
