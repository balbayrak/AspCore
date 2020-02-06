using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.Dependency.Concrete;

namespace AspCore.Dependency.Configuration
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Uygulamada ITransientType,IScopedType,ISingletonType olarak tanımlanmış sınıflar verilen namespace de bulunana classlar ile otomatik olarak bind edilir.
        /// namespace null olarak gönderilirse ilgili katmandaki bütün classlar otomatik olarak bind edilir.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="namespaceStr"></param>
        /// <returns></returns>
        public static IServiceCollection AutoBind(this IServiceCollection services, Action<DependencyOption> option)
        {
            using (DependencyOptionBuilder builder = new DependencyOptionBuilder(services))
            {
                builder.AutoBind(option);
                return services;
            }
        }

        public static IServiceCollection Bind<TInterface>(this IServiceCollection services, Action<DependencyOption> option)
        {
            using (DependencyOptionBuilder builder = new DependencyOptionBuilder(services))
            {
                builder.Bind<TInterface>(option);
                return services;
            }
        }

        /// <summary>
        /// Bind edilmek istenen TInterface yada Tconcrete daha önceden bind edilmiş ise yeni type ile değiştirilir.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        /// <param name="services"></param>
        /// <param name="lifeTime"></param>
        /// <returns></returns>
        public static IServiceCollection BindType<TInterface, TConcrete>(this IServiceCollection services, Action<DependencyOption> option)
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
