using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AspCore.AOP.Concrete;
using AspCore.Dependency.Abstract;

namespace AspCore.AOP.Abstract
{
    public interface IProxySelector : ISingletonType
    {
        bool ShouldInterceptMethod(Type type, MethodInfo methodInfo);

        bool ShouldInterceptType(Type type);

        List<InterceptorType> GetInterceptMethodInterceptors(Type type, MethodInfo methodInfo);

        List<InterceptorType> GetInterceptTypeInterceptors(Type type);

    }
}
