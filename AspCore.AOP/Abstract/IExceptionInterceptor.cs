using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.AOP.Abstract
{
    public interface IExceptionInterceptor
    {
        void OnException(Exception exception);
    }
}
