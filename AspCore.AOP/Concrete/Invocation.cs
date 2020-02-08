using AspCore.AOP.Abstract;
using System.Reflection;

namespace AspCore.AOP.Concrete
{
    public class Invocation : IInvocation
    {
        public object ImplementationObj { get; set; }
        public MethodInfo targetMethod { get; set; }
        public object[] args { get; set; }
        public object result { get; set; }

        private bool _isProceeded { get; set; }

        public bool isProceeded
        {
            get
            {
                return _isProceeded;
            }
        }


        public Invocation()
        {
            this.args = null;
            this.targetMethod = null;
            _isProceeded = false;
        }

        public Invocation(MethodInfo targetMethod, object[] args, object impObj)
        {
            this.targetMethod = targetMethod;
            this.args = args;
            this.ImplementationObj = impObj;
            _isProceeded = false;
        }

        public void Proceed()
        {
            if (!_isProceeded)
            {
                _isProceeded = true;
                result = targetMethod.Invoke(ImplementationObj, args);
            }
        }
    }
}
