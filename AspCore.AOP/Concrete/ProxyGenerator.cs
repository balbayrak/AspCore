using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AspCore.AOP.Abstract;

namespace AspCore.AOP.Concrete
{
    public class ProxyGenerator : DispatchProxy, IProxyGenerator
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

        private void SetParameters(object ImplementationObj, IInterceptorContext context, IProxySelector proxySelector, Type serviceType, Type impType)
        {
            this._implementationObj = ImplementationObj;
            this._context = context;
            this._proxySelector = proxySelector;
            this._serviceType = serviceType;
            this._impType = impType;
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
                    // Before aspectlerimizi çalıştırıyoruz önce ve geriye değer dönen varsa respons'a eşitliyoruz.
                    CheckBeforeInterceptor(interceptors);

                    // Response boş değilse, buradaki veri cache üzerinden de geliyor olabilir ve tekrardan invoke etmeye
                    // gerek yok, direkt olarak geriye response dönebiliriz bu durumda.
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

        private List<InterceptorType> GetInterceptorTypes(MethodInfo targetMethod, Type serviceType, Type implementationType)
        {
            List<InterceptorType> list = _proxySelector.GetInterceptMethodInterceptors(implementationType, targetMethod);
            list.AddRange(_proxySelector.GetInterceptTypeInterceptors(implementationType));

            list.AddRange(_proxySelector.GetInterceptMethodInterceptors(serviceType, targetMethod));
            list.AddRange(_proxySelector.GetInterceptTypeInterceptors(serviceType));

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
    }
}