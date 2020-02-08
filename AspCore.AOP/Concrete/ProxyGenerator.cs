using AspCore.AOP.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspCore.AOP.Concrete
{
    public class ProxyGenerator : DispatchProxy, IProxyGenerator, IDisposable
    {
        private object _implementationObj { get; set; }
        private IInterceptorContext _context { get; set; }

        private IProxySelector _proxySelector;

        private Type _serviceType { get; set; }
        private Type _impType { get; set; }

        public ProxyGenerator()
        {

        }
        public ProxyGenerator(IProxySelector proxySelector)
        {
            _proxySelector = proxySelector;
        }

        public object Create(Type serviceType, Type implementationType, object implementationObj, IInterceptorContext context)
        {
            MethodInfo method = typeof(DispatchProxy).GetMethod("Create");
            var proxyType = typeof(ProxyGenerator);
            MethodInfo genericMethod = method.MakeGenericMethod(serviceType, proxyType);

            //service dispatcher ile birlikte açılır, her method fire olduğunda dispatcher araya girer.
            object service = genericMethod.Invoke(null, null);
            ((ProxyGenerator)service).SetParameters(implementationObj, context, _proxySelector, serviceType, implementationType);
            return service;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            List<InterceptorType> interceptors = GetInterceptorTypes(targetMethod, _serviceType, _impType);
            if (interceptors != null && interceptors.Count > 0)
            {
                try
                {
                    object response = null;

                    _context.invocation = _context.invocation ?? new Invocation(targetMethod, args, _implementationObj);
                    _context.invocation.result = null;

                    // run OnBefore method of before interceptor
                    CheckBeforeInterceptor(interceptors);

                    //if method proceeded in interceptor, not work again.
                    //forexample, cache interceptor run before, result gets from cache and function not need to work
                    if (response == null && !_context.invocation.isProceeded)
                    {
                        response = targetMethod.Invoke(_implementationObj, args);
                        _context.invocation.result = response;
                    }

                    CheckAfterInterceptor(interceptors);
                }
                catch (Exception ex)
                {
                    CheckExceptionInterceptor(ex, interceptors);
                }
            }

            return _context.invocation.result;
        }
        private void SetParameters(object ImplementationObj, IInterceptorContext context, IProxySelector proxySelector, Type serviceType, Type impType)
        {
            this._implementationObj = ImplementationObj;
            this._context = context;
            this._proxySelector = proxySelector;
            this._serviceType = serviceType;
            this._impType = impType;
        }

        private List<InterceptorType> GetInterceptorTypes(MethodInfo targetMethod, Type serviceType, Type implementationType)
        {
            List<InterceptorType> list = _proxySelector.GetInterceptMethodInterceptors(implementationType, targetMethod);
            list = list ?? new List<InterceptorType>();

            List<InterceptorType> implementationTypeInterceptors = _proxySelector.GetInterceptTypeInterceptors(implementationType);
            list.AddRange(implementationTypeInterceptors);

            List<InterceptorType> serviceTypeInterceptors = _proxySelector.GetInterceptTypeInterceptors(serviceType);
            list.AddRange(serviceTypeInterceptors);

            return list.Distinct().OrderBy(t => t.priority).ToList();
        }

        private void CheckBeforeInterceptor(List<InterceptorType> interceptors)
        {
            try
            {
                List<InterceptorType> beforeInterceptors = interceptors.Where(t => t.runType == EnumInterceptorRunType.Before ||
                  t.runType == EnumInterceptorRunType.BeforeAfter).ToList();
                foreach (InterceptorType item in beforeInterceptors)
                {
                    var instantiatedObject = (IBeforeInterceptor)Activator.CreateInstance(item.type, _context);
                    instantiatedObject.OnBefore();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckAfterInterceptor(List<InterceptorType> interceptors)
        {
            try
            {
                List<InterceptorType> afterInterceptors = interceptors.Where(t => t.runType == EnumInterceptorRunType.After ||
                  t.runType == EnumInterceptorRunType.BeforeAfter).ToList();
                foreach (InterceptorType item in afterInterceptors)
                {
                    var instantiatedObject = (IAfterInterceptor)Activator.CreateInstance(item.type, _context);
                    instantiatedObject.OnAfter();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckExceptionInterceptor(Exception exception, List<InterceptorType> interceptors)
        {
            try
            {
                List<InterceptorType> exceptionInterceptors = interceptors.Where(t => t.runType == EnumInterceptorRunType.Exception).ToList();
                foreach (InterceptorType item in exceptionInterceptors)
                {
                    var instantiatedObject = (IExceptionInterceptor)Activator.CreateInstance(item.type, _context);
                    instantiatedObject.OnException(exception);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            this._context = null;
            this._implementationObj = null;
        }
    }
}