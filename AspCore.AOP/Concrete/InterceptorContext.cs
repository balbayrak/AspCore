using AspCore.AOP.Abstract;

namespace AspCore.AOP.Concrete
{
    public class InterceptorContext : IInterceptorContext
    {
        public IInvocation invocation { get; set; }
    }
}
