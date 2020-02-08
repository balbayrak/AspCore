using AspCore.Dependency.Abstract;

namespace AspCore.AOP.Abstract
{
    public interface IInterceptorContext : IScopedType
    {
        IInvocation invocation { get; set; }
    }
}
