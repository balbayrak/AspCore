using System.Reflection;

namespace AspCore.AOP.Abstract
{
    public interface IInvocation
    {
        MethodInfo targetMethod { get; set; }
        object[] args { get; set; }

        object result { get; set; }

        bool isProceeded { get; }

        void Proceed();
    }
}
