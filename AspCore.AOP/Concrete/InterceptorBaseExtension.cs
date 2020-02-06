using System;
using System.Collections.Generic;
using System.Text;
using AspCore.AOP.Abstract;

namespace AspCore.AOP.Concrete
{
    public static class InterceptorBaseExtension
    {
        public static InterceptorType GetInterceptorType(this InterceptorBase interceptorBase)
        {
            InterceptorType interceptorType = new InterceptorType();
            interceptorType.priority = interceptorBase.priority;
            interceptorType.type = interceptorBase.GetType();
            if (typeof(IBeforeInterceptor).IsAssignableFrom(interceptorType.type) && typeof(IAfterInterceptor).IsAssignableFrom(interceptorType.type))
            {
                interceptorType.runType = EnumInterceptorRunType.BeforeAfter;
            }
            else if(typeof(IAfterInterceptor).IsAssignableFrom(interceptorType.type))
            {
                interceptorType.runType = EnumInterceptorRunType.After;
            }
            else if (typeof(IBeforeInterceptor).IsAssignableFrom(interceptorType.type))
            {
                interceptorType.runType = EnumInterceptorRunType.Before;
            }
            else if (typeof(IExceptionInterceptor).IsAssignableFrom(interceptorType.type))
            {
                interceptorType.runType = EnumInterceptorRunType.Exception;
            }

            return interceptorType;
        }
    }
}
