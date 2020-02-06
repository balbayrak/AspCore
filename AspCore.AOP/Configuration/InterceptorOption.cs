using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using AspCore.AOP.Abstract;
using AspCore.AOP.Concrete;
using AspCore.Dependency.Abstract;
using AspCore.Dependency.Concrete;

namespace AspCore.AOP.Configuration
{
    public class InterceptorOption
    {
        public string namespaceStr { get; set; }

        public InterceptorOption()
        {
            namespaceStr = null;
        }

        public void AddInterceptors(IServiceCollection services, Action<InterceptorOption> option = null)
        {
            string nameSpaceStr = null;
            if (option != null)
            {
                InterceptorOption interceptorOption = new InterceptorOption();
                option.Invoke(interceptorOption);
                nameSpaceStr = interceptorOption.namespaceStr;
            }


            var proxySelectorCnt = services.FirstOrDefault(t => t.ServiceType.Equals(typeof(IProxySelector)));
            if (proxySelectorCnt == null)
            {
                services.AddSingleton(typeof(IProxySelector), new AttributeBaseProxySelector());
            }

            var interceptorContextCnt = services.FirstOrDefault(t => t.ServiceType.Equals(typeof(IProxySelector)));
            if (interceptorContextCnt == null)
            {
                services.AddScoped<IInterceptorContext, InterceptorContext>();
            }

            var proxyGeneratorCnt = services.FirstOrDefault(t => t.ServiceType.Equals(typeof(IProxySelector)));
            if (proxyGeneratorCnt == null)
            {
                services.AddScoped<IProxyGenerator, ProxyGenerator>();
            }

            BindInterceptorType<ITransientType>(services, nameSpaceStr);
            BindInterceptorType<IScopedType>(services, nameSpaceStr);
            BindInterceptorType<ISingletonType>(services, nameSpaceStr);

        }

        public void AddInterceptors<TProxySelector>(IServiceCollection services, Action<InterceptorOption> option = null)
              where TProxySelector : class, IProxyGenerator, new()
        {
            string nameSpaceStr = null;

            if (option != null)
            {
                InterceptorOption interceptorOption = new InterceptorOption();
                option.Invoke(interceptorOption);
                nameSpaceStr = interceptorOption.namespaceStr;
            }


            var proxySelectorCnt = services.FirstOrDefault(t => t.ServiceType.Equals(typeof(IProxySelector)));
            if (proxySelectorCnt == null)
            {
                services.AddSingleton(typeof(IProxySelector), typeof(TProxySelector));
            }

            var interceptorContextCnt = services.FirstOrDefault(t => t.ServiceType.Equals(typeof(IProxySelector)));
            if (interceptorContextCnt == null)
            {
                services.AddScoped<IInterceptorContext, InterceptorContext>();
            }

            var proxyGeneratorCnt = services.FirstOrDefault(t => t.ServiceType.Equals(typeof(IProxySelector)));
            if (proxyGeneratorCnt == null)
            {
                services.AddScoped<IProxyGenerator, ProxyGenerator>();
            }

            BindInterceptorType<ITransientType>(services, nameSpaceStr);
            BindInterceptorType<IScopedType>(services, nameSpaceStr);
            BindInterceptorType<ISingletonType>(services, nameSpaceStr);

        }

        private static void BindInterceptorType<TInterface>(IServiceCollection services, string namespaceStr = null)
        {
            IEnumerable<TypeMap> maps = TypeMapHelper.GetTypeMaps<TInterface>(AppDomain.CurrentDomain.GetAssemblies());

            foreach (var typeMap in maps)
            {
                var implementationType = typeMap.ImplementationType;

                var descriptor = new ServiceDescriptor(implementationType, implementationType, ServiceLifetime.Scoped);
                services.Add(descriptor);


                foreach (var serviceType in typeMap.ServiceTypes)
                {
                    var oldDescription = services.FirstOrDefault(t => t.ServiceType.Equals(serviceType));
                    AddType(services, serviceType, implementationType, oldDescription);
                }
            }
        }

        private static void AddType(IServiceCollection services, Type serviceType, Type implementationType, ServiceDescriptor oldDescription)
        {
            if (oldDescription.Lifetime == ServiceLifetime.Transient)
            {
                services.AddTransient(serviceType, sp =>
                {
                    return AddType(services, sp, serviceType, implementationType, oldDescription);
                });
            }
            else if (oldDescription.Lifetime == ServiceLifetime.Scoped)
            {
                services.AddScoped(serviceType, sp =>
                {
                    return AddType(services, sp, serviceType, implementationType, oldDescription);
                });
            }
            else if (oldDescription.Lifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton(serviceType, sp =>
                {
                    return AddType(services, sp, serviceType, implementationType, oldDescription);
                });
            }
        }

        private static object AddType(IServiceCollection services, IServiceProvider sp, Type serviceType, Type implementationType, ServiceDescriptor oldDescription = null)
        {
            IProxySelector proxySelector = (IProxySelector)sp.GetRequiredService<IProxySelector>();

            bool isIncludeAspect = proxySelector.ShouldInterceptType(implementationType);

            if (!implementationType.IsAssignableTo(serviceType))
            {
                throw new InvalidOperationException($@"Type ""{implementationType.ToFriendlyName()}"" is not assignable to ""${serviceType.ToFriendlyName()}"".");
            }
            if (!isIncludeAspect) isIncludeAspect = proxySelector.ShouldInterceptType(serviceType);

            if (isIncludeAspect)
            {
                if (oldDescription != null)
                {
                    services.Remove(oldDescription);
                }

                object obj = sp.GetRequiredService(implementationType);
                IProxyGenerator objProxy = sp.GetRequiredService<IProxyGenerator>();
                IInterceptorContext context = sp.GetRequiredService<IInterceptorContext>();

                return objProxy.Create(serviceType, implementationType, obj, context);
            }
            else
            {
                return sp.GetRequiredService(serviceType);
            }
        }

    }
}
