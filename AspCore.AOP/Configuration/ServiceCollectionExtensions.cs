using AspCore.AOP.Abstract;
using AspCore.AOP.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.AOP.Concrete
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Types which implements ITransientType,IScopedType,ISingletonType interfaces, inject with proxygenerator automatically in this method.
        /// ProxySelector used AttributeBaseProxySelector as default. 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IServiceCollection AutoBindWithInterceptors(this IServiceCollection services, Action<InterceptorOption> option = null)
        {
            using (InterceptorOptionBuilder builder = new InterceptorOptionBuilder())
            {
                builder.BindWithInterceptors(services, option);
                return services;
            }
        }

        /// <summary>
        /// Types which implements ITransientType,IScopedType,ISingletonType interfaces, inject with proxygenerator automatically in this method.
        /// ProxySelector used with custom class.
        /// </summary>
        /// <typeparam name="TProxySelector"></typeparam>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IServiceCollection AutoBindWithInterceptors<TProxySelector>(this IServiceCollection services, Action<InterceptorOption> option = null)
              where TProxySelector : class, IProxySelector, new()
        {
            using (InterceptorOptionBuilder builder = new InterceptorOptionBuilder())
            {
                builder.BindWithInterceptors<TProxySelector>(services, option);
                return services;
            }
        }
    }
}
