using AspCore.AOP.Concrete;
using AspCore.Dependency.Abstract;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AspCore.AOP.Abstract
{
    public interface IProxySelector : ISingletonType, IDisposable
    {
        bool ShouldInterceptMethod(Type type, MethodInfo methodInfo);

        bool ShouldInterceptType(Type type);

        bool ShouldInterceptTypes(List<Type> types);

        List<InterceptorType> GetInterceptMethodInterceptors(Type type, MethodInfo methodInfo);

        List<InterceptorType> GetInterceptTypeInterceptors(Type type);

    }
}
