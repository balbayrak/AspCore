using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.ApiClient.Entities;
using AspCore.ApiClient.Entities.Abstract;
using AspCore.Storage.Abstract;

namespace AspCore.ApiClient.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureApiClientStorage(this IServiceCollection services, Action<ApiClientOption> option)
        {
            using (ApiClientStorageBuilder builder = new ApiClientStorageBuilder(services))
            {
                builder.AddApiClientStorage(option);
            }

            return services;
        }

        public static IServiceCollection ConfigureApiClientWithCustomStorage<TStorageService>(this IServiceCollection services, Action<ApiClientOption> option)
             where TStorageService : class, IStorage, new()
        {
            using (ApiClientStorageBuilder builder = new ApiClientStorageBuilder(services))
            {
                builder.AddApiClientStorage<TStorageService>(option);
            }

            return services;
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "IApiClientConfiguration" type.
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static void AddApiClient<TOption>(this IServiceCollection services, string apiKey)
            where TOption : class, IApiClientConfiguration, new()
        {
            services.AddTransient(typeof(ApiClient<TOption>), sp =>
            {
                return new ApiClient<TOption>(apiKey);
            });

        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static void AddApiClient(this IServiceCollection services, string apiKey)
        {
            services.AddTransient(typeof(ApiClient<ApiClientConfiguration>), sp =>
            {
                return new ApiClient<ApiClientConfiguration>(apiKey);
            });
        }


        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="apiKey"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static void AddBearerAuthenticatedClient<TOption>(this IServiceCollection services, string apiKey)
          where TOption : class, IApiClientConfiguration, new()
        {
            services.AddTransient(typeof(BearerAuthenticatedApiClient<TOption>), sp =>
            {
                return new BearerAuthenticatedApiClient<TOption>(apiKey);
            });
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static void AddBearerAuthenticatedClient(this IServiceCollection services, string apiKey)
        {
            services.AddTransient(typeof(BearerAuthenticatedApiClient<ApiClientConfiguration>), sp =>
            {
                return new BearerAuthenticatedApiClient<ApiClientConfiguration>(apiKey);
            });
        }


        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static void AddJWTAuthenticatedClient<TOption>(this IServiceCollection services, string apiKey)
            where TOption : class, IApiClientConfiguration, new()
        {
            services.AddTransient(typeof(JWTAuthenticatedApiClient<TOption>), sp =>
            {
                return new JWTAuthenticatedApiClient<TOption>(apiKey);
            });
        }


        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static void AddJWTAuthenticatedClient(this IServiceCollection services, string apiKey)
        {
            services.AddTransient(typeof(JWTAuthenticatedApiClient<ApiClientConfiguration>), sp =>
            {
                return new JWTAuthenticatedApiClient<ApiClientConfiguration>(apiKey);
            });
        }
    }
}
