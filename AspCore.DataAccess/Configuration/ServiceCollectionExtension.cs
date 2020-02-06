using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.DataAccess.Configuration;
using AspCore.DataAccess.EntityFramework;
using AspCore.Entities.Configuration;

namespace AspCore.DataAccess.Extension
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// IDataAccessSetting configuration bilgileri startup içerinden set edilebilir yada configuration name verilerek otomatik set edilebilir
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        public static IServiceCollection ConfigureDataAccessLayer<TDbContext>(this IServiceCollection services, string configurationKey, Action<DataAccessLayerOption> option = null)
           where TDbContext : CoreDbContext
        {
            DataAccessLayerOptionBuilder builder = new DataAccessLayerOptionBuilder(services);
            builder.ConfigureDataAccessLayer<TDbContext>(configurationKey, option);

            return services;
        }

        /// <summary>
        /// IDataAccessSetting configuration bilgileri startup içerinden set edilebilir yada configuration name verilerek otomatik set edilebilir
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>

        public static IServiceCollection ConfigureDataAccessLayer<TDbContext, TOption>(this IServiceCollection services,string configurationKey, Action<TOption> option = null)
            where TDbContext : CoreDbContext
            where TOption : class, IConfigurationEntity, IDataAccessLayerOption, new()
        {
            DataAccessLayerOptionBuilder builder = new DataAccessLayerOptionBuilder(services);
            builder.ConfigureDataAccessLayer<TDbContext, TOption>(configurationKey, option);

            return services;
        }
    }
}
