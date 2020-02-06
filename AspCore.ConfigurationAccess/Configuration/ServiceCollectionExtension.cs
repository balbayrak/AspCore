using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.ConfigurationAccess.Abstract;

namespace AspCore.ConfigurationAccess.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddConfigurationManager(this IServiceCollection services, Action<ConfigurationManagerOption> option)
        {
            using (ConfigurationManagerOptionBuilder builder = new ConfigurationManagerOptionBuilder(services))
            {
                builder.AddConfigurationHelper(option);
                return services;
            }
        }

        public static IServiceCollection AddConfigurationManager<T>(this IServiceCollection services, Action<ConfigurationManagerOption> option)
             where T : class, IConfigurationHelper, new()
        {

            using (ConfigurationManagerOptionBuilder builder = new ConfigurationManagerOptionBuilder(services))
            {
                builder.AddConfigurationHelper<T>(option);
                return services;
            }
        }
    }
}
