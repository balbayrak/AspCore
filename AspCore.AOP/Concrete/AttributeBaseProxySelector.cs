using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AspCore.AOP.Abstract;

namespace AspCore.AOP.Concrete
{
    public class AttributeBaseProxySelector : IProxySelector
    {
        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            MethodInfo method = type.GetMethods().FirstOrDefault(t => t.Name.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase));
            if (method != null)
            {
                var methodAttributes = method.GetCustomAttributes<InterceptorBase>(true);
                if (methodAttributes != null && methodAttributes.Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ShouldInterceptType(Type type)
        {
            var classAttributes = type.GetCustomAttributes<InterceptorBase>(true);
            if (classAttributes != null && classAttributes.Count() > 0)
            {
                return true;
            }
            MethodInfo[] methods = type.GetMethods();

            foreach (var method in methods)
            {
                var methodAttributes = method.GetCustomAttributes<InterceptorBase>(true);
                if (methodAttributes != null && methodAttributes.Count() > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public List<InterceptorType> GetInterceptMethodInterceptors(Type type, MethodInfo methodInfo)
        {
            MethodInfo method = type.GetMethods().FirstOrDefault(t => t.Name.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase));
            if (method != null)
            {
                return method.GetCustomAttributes<InterceptorBase>(true).Select(t => t.GetInterceptorType()).ToList();
            }
            return null;

        }

        public List<InterceptorType> GetInterceptTypeInterceptors(Type type)
        {
            return type.GetCustomAttributes<InterceptorBase>(true).Select(t => t.GetInterceptorType()).ToList();
        }
    }
}
