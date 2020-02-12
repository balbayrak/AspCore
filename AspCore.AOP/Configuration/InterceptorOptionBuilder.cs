using AspCore.AOP.Abstract;
using AspCore.AOP.Concrete;
using AspCore.Dependency.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Dependency.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.AOP.Configuration
{
    public class InterceptorOptionBuilder : DependencyOptionBuilder
    {
        public InterceptorOptionBuilder(IServiceCollection services) : base(services)
        {
        }

        public void BindWithInterceptors(IServiceCollection services, Action<InterceptorOption> option = null)
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


            BindType<ITransientType, AttributeBaseProxySelector>(services, nameSpaceStr);
            BindType<IScopedType, AttributeBaseProxySelector>(services, nameSpaceStr);
            BindType<ISingletonType, AttributeBaseProxySelector>(services, nameSpaceStr);

        }

        public void BindWithInterceptors<TProxySelector>(IServiceCollection services, Action<InterceptorOption> option = null)
              where TProxySelector : class, IProxySelector, new()
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

            BindType<ITransientType, TProxySelector>(services, nameSpaceStr);
            BindType<IScopedType, TProxySelector>(services, nameSpaceStr);
            BindType<ISingletonType, TProxySelector>(services, nameSpaceStr);

        }

        private static void BindType<TInterface, TProxySelector>(IServiceCollection services, string namespaceStr = null)
             where TProxySelector : class, IProxySelector, new()
        {

            using (TProxySelector proxySelector = new TProxySelector())
            {
                ServiceLifetime lifetime = ServiceLifetime.Scoped;
                if (typeof(TInterface) == typeof(ISingletonType))
                {
                    lifetime = ServiceLifetime.Singleton;
                }
                else if (typeof(TInterface) == typeof(ITransientType))
                {
                    lifetime = ServiceLifetime.Transient;
                }

                IEnumerable<TypeMap> maps = TypeMapHelper.GetTypeMaps<TInterface>(AppDomain.CurrentDomain.GetAssemblies(), namespaceStr);

                foreach (var typeMap in maps)
                {
                    var implementationType = typeMap.ImplementationType;

                    var types = typeMap.ServiceTypes.Where(t => t != typeof(IProxySelector) &&
                             t != typeof(IInterceptorContext) &&
                             t != typeof(IProxyGenerator) &&
                             t != typeof(ISingletonType) &&
                             t != typeof(IScopedType) &&
                             t != typeof(ITransientType)).ToList();

                    types.Add(typeMap.ImplementationType);

                    var oldDescriptionImp = services.FirstOrDefault(t => t.ServiceType == implementationType);
                    if (oldDescriptionImp != null)
                    {
                        services.Remove(oldDescriptionImp);
                    }

                    bool isIncludeAspect = proxySelector.ShouldInterceptTypes(types);

                    if (isIncludeAspect)
                    {
                        BindInterceptorTypeMap<TInterface>(services, typeMap, lifetime);
                    }
                    else
                    {
                        BindTypeMap<TInterface>(services, typeMap, lifetime);
                    }
                }
            }
        }

        private static void BindTypeMap<TInterface>(IServiceCollection services, TypeMap typeMap, ServiceLifetime lifeTime)
        {
            var implementationType = typeMap.ImplementationType;

            foreach (var serviceType in typeMap.ServiceTypes)
            {
                if (!implementationType.IsAssignableTo(serviceType))
                {
                    throw new InvalidOperationException($@"Type ""{implementationType.ToFriendlyName()}"" is not assignable to ""${serviceType.ToFriendlyName()}"".");
                }

                var descriptor = new ServiceDescriptor(serviceType, implementationType, lifeTime);

                var oldDescription = services.FirstOrDefault(t => t.ServiceType == typeof(TInterface));
                if (oldDescription != null)
                {
                    services.Remove(oldDescription);
                }

                services.Add(descriptor);
            }
        }

        private static void BindInterceptorTypeMap<TInterface>(IServiceCollection services, TypeMap typeMap, ServiceLifetime lifetime)
        {
            var implementationType = typeMap.ImplementationType;

            var serviceTypes = typeMap.ServiceTypes.Where(t => t != typeof(IProxySelector) &&
                     t != typeof(IInterceptorContext) &&
                     t != typeof(IProxyGenerator) &&
                     t != typeof(ISingletonType) &&
                     t != typeof(IScopedType) &&
                     t != typeof(ITransientType)).ToList();

            foreach (var serviceType in serviceTypes)
            {
                var oldDescription = services.FirstOrDefault(t => t.ServiceType == typeof(TInterface));
                if (oldDescription != null)
                {
                    services.Remove(oldDescription);
                }

                services.Add(new ServiceDescriptor(implementationType, implementationType, lifetime));

                services.AddTransient(serviceType, sp =>
                {
                    IProxySelector proxySelector = sp.GetRequiredService<IProxySelector>();

                    bool isIncludeAspect = proxySelector.ShouldInterceptType(implementationType);

                    if (!implementationType.IsAssignableTo(serviceType))
                    {
                        throw new InvalidOperationException($@"Type ""{implementationType.ToFriendlyName()}"" is not assignable to ""${serviceType.ToFriendlyName()}"".");
                    }

                    object obj = sp.GetRequiredService(implementationType);
                    IProxyGenerator objProxy = new ProxyGenerator(proxySelector);
                    IInterceptorContext context = new InterceptorContext();
                    return objProxy.Create(serviceType, implementationType, obj, context);

                });
            }
        }

        public void Dispose()
        {
        }
    }
}
