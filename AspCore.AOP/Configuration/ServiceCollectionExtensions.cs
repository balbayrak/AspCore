using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AspCore.AOP.Abstract;
using AspCore.AOP.Configuration;
using AspCore.Dependency.Abstract;
using AspCore.Dependency.Concrete;

namespace AspCore.AOP.Concrete
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// ITransientType,IScopedType,ISingleton tiplerinde seçilen ProxySelector ile interceptor ataması otomatik olarak yapılır.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="namespaceStr"></param>
        /// <returns></returns>
        public static IServiceCollection BindInterceptors(this IServiceCollection services, Action<InterceptorOption> option = null)
        {
            using (InterceptorOptionBuilder builder = new InterceptorOptionBuilder())
            {
                builder.AddInterceptors(services, option);
                return services;
            }

        }

        public static IServiceCollection BindInterceptors<TProxySelector>(this IServiceCollection services, Action<InterceptorOption> option = null)
              where TProxySelector : class, IProxyGenerator, new()
        {
            using (InterceptorOptionBuilder builder = new InterceptorOptionBuilder())
            {
                builder.AddInterceptors<TProxySelector>(services, option);
                return services;
            }
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
