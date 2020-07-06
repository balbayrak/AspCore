using AspCore.ApiClient.Entities;
using AspCore.ApiClient.Entities.Abstract;
using AspCore.Caching.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.ApiClient.Abstract;
using System.Net.Http;

namespace AspCore.ApiClient.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureApiClientStorage(this IServiceCollection services, Action<ApiClientCacheBuilder> option)
        {
            using (ApiClientCacheBuilder builder = new ApiClientCacheBuilder(services))
            {
                option(builder);
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
                var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                var cacheService = sp.GetRequiredService<ICacheService>();
                var cancellationTokenHelper = sp.GetRequiredService<ICancellationTokenHelper>();

                return new ApiClient<TOption>(httpClientfactory,httpContextAccessor, configurationAccessor, cacheService, cancellationTokenHelper, apiKey);
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
                var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                var cacheService = sp.GetRequiredService<ICacheService>();
                var cancellationTokenHelper = sp.GetRequiredService<ICancellationTokenHelper>();

                return new ApiClient<ApiClientConfiguration>(httpClientfactory,httpContextAccessor, configurationAccessor, cacheService, cancellationTokenHelper, apiKey);
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
                var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                var cacheService = sp.GetRequiredService<ICacheService>();
                var cancellationTokenHelper = sp.GetRequiredService<ICancellationTokenHelper>();
                return new BearerAuthenticatedApiClient<TOption>(httpClientfactory,httpContextAccessor, configurationAccessor, cacheService, cancellationTokenHelper, apiKey);
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
                var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                var cacheService = sp.GetRequiredService<ICacheService>();
                var cancellationTokenHelper = sp.GetRequiredService<ICancellationTokenHelper>();
                return new BearerAuthenticatedApiClient<ApiClientConfiguration>(httpClientfactory,httpContextAccessor, configurationAccessor, cacheService, cancellationTokenHelper, apiKey);
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
                var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                var cacheService = sp.GetRequiredService<ICacheService>();
                var cancellationTokenHelper = sp.GetRequiredService<ICancellationTokenHelper>();
                return new JWTAuthenticatedApiClient<TOption>(httpClientfactory,httpContextAccessor, configurationAccessor, cacheService, cancellationTokenHelper, apiKey);
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
                var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                var cacheService = sp.GetRequiredService<ICacheService>();
                var cancellationTokenHelper = sp.GetRequiredService<ICancellationTokenHelper>();
                return new JWTAuthenticatedApiClient<ApiClientConfiguration>(httpClientfactory,httpContextAccessor, configurationAccessor, cacheService, cancellationTokenHelper, apiKey);
            });
        }
    }
}
