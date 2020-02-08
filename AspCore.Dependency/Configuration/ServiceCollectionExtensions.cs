using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.Dependency.Concrete;

namespace AspCore.Dependency.Configuration
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Types which implements ITransientType,IScopedType,ISingletonType interfaces, inject automatically in this method.
        /// If namepace is null, all classes injects, but if namespace is not null classes inject only in this namepace.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IServiceCollection AutoBind(this IServiceCollection services, Action<DependencyOption> option = null)
        {
            using (DependencyOptionBuilder builder = new DependencyOptionBuilder(services))
            {
                builder.AutoBind(option);
                return services;
            }
        }

        public static IServiceCollection Bind<TInterface>(this IServiceCollection services, Action<DependencyOption> option = null)
        {
            using (DependencyOptionBuilder builder = new DependencyOptionBuilder(services))
            {
                builder.Bind<TInterface>(option);
                return services;
            }
        }

        /// <summary>
        /// Inject class to interface with ServiceLifetime.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IServiceCollection BindType<TInterface, TConcrete>(this IServiceCollection services, Action<DependencyOption> option = null)
        {
            using (DependencyOptionBuilder builder = new DependencyOptionBuilder(services))
            {
                builder.Bind<TInterface, TConcrete>(option);
                return services;
            }
        }

        public static ServicesByNameBuilder<TService> BindTransientByName<TService>(this IServiceCollection services)
        {
            return new ServicesByNameBuilder<TService>(services, ServiceLifetime.Transient);
        }
      
        public static ServicesByNameBuilder<TService> BindScopedByName<TService>(this IServiceCollection services)
        {
            return new ServicesByNameBuilder<TService>(services, ServiceLifetime.Scoped);
        }
       
        public static ServicesByNameBuilder<TService> BindSingletonByName<TService>(this IServiceCollection services)
        {
            return new ServicesByNameBuilder<TService>(services, ServiceLifetime.Singleton);
        }
    }
}
