using System;

namespace AspCore.AOP.Abstract
{
    public interface IExceptionInterceptor : IInterceptor
    {
        void OnException(Exception exception);
    }
}
