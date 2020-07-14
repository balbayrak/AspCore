using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities;
using AspCore.ApiClient.Entities.Abstract;
using AspCore.Caching.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

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

        public static string AddApiClient<TOption>(this IServiceCollection services, Action<ApiClientOption> clientOption)
            where TOption : class, IApiClientConfiguration, new()
        {
            return AddClient<ApiClient<TOption>>(services, clientOption);
        }

        public static string AddApiClient(this IServiceCollection services, Action<ApiClientOption> clientOption)
        {
            return AddClient<ApiClient<ApiClientConfiguration>>(services, clientOption);
        }

        public static string AddBearerAuthenticatedClient<TOption>(this IServiceCollection services, Action<ApiClientOption> clientOption)
          where TOption : class, IApiClientConfiguration, new()
        {
            return AddClient<BearerAuthenticatedApiClient<TOption>>(services, clientOption);
        }

        public static string AddBearerAuthenticatedClient(this IServiceCollection services, Action<ApiClientOption> clientOption)
        {
            return AddClient<BearerAuthenticatedApiClient<ApiClientConfiguration>>(services, clientOption);
        }

        public static string AddJWTAuthenticatedClient<TOption>(this IServiceCollection services, Action<ApiClientOption> clientOption)
            where TOption : class, IApiClientConfiguration, new()
        {
            return AddClient<JWTAuthenticatedApiClient<TOption>>(services, clientOption);
        }

        public static string AddJWTAuthenticatedClient(this IServiceCollection services, Action<ApiClientOption> clientOption)
        {
            return AddClient<JWTAuthenticatedApiClient<ApiClientConfiguration>>(services, clientOption);
        }

        public static void AddClient<T, TConcrete>(this IServiceCollection services, Action<ApiClientOption> apiClientOption)
          where T : IApiClient
          where TConcrete : ApiClient<ApiClientConfiguration>

        {
            AddClient<T, TConcrete, ApiClientConfiguration>(services, apiClientOption);
        }

        public static void AddClient<T, TConcrete, TOption>(this IServiceCollection services, Action<ApiClientOption> clientOption)
           where T : IApiClient
           where TConcrete : ApiClient<TOption>
           where TOption : class, IApiClientConfiguration, new()

        {
            ApiClientOption apiClientOption = new ApiClientOption();
            clientOption(apiClientOption);

            AddClientConfiguration(services, apiClientOption);

            services.AddTransient(typeof(T), sp =>
            {
                var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                var cacheService = sp.GetRequiredService<ICacheService>();
                var cancellationTokenHelper = sp.GetRequiredService<ICancellationTokenHelper>();

                return (TConcrete)Activator.CreateInstance(typeof(TConcrete), httpClientfactory, httpContextAccessor, configurationAccessor, cacheService, cancellationTokenHelper, apiClientOption.apiKey);
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
        {
            return HttpPolicyExtensions
              // Handle HttpRequestExceptions, 408 and 5xx status codes
              .HandleTransientHttpError()
              // Handle 404 not found
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              // Handle 401 Unauthorized
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
              // What to do if any of the above erros occur:
              // Retry 3 times, each time wait 5,10 and 20 seconds before retrying.
              .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(10, retryAttempt)));
        }

        private static void AddClientConfiguration(IServiceCollection services, ApiClientOption apiClientOption)
        {
            services.AddHttpClient(apiClientOption.apiKey, client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstants.Api_Keys.JSON_MEDIA_TYPE_QUALITY_HEADER));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(ApiConstants.Api_Keys.GZIP_COMPRESSION_STRING_WITH_QUALITY_HEADER));

            })
           .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMinutes(apiClientOption.timeout)))
           .AddPolicyHandler(GetRetryPolicy(apiClientOption.retryCount))
           .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(apiClientOption.circuitbreakerCount, TimeSpan.FromSeconds(30)))
           .AddHeaderPropagation().ConfigurePrimaryHttpMessageHandler(() =>
           {
               return new HttpClientHandler()
               {
                   AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
               };
           });

        }

        private static string AddClient<TConcrete>(IServiceCollection services, Action<ApiClientOption> clientOption)
            where TConcrete : class, IApiClient
        {
            ApiClientOption apiClientOption = new ApiClientOption();
            clientOption(apiClientOption);

            AddClientConfiguration(services, apiClientOption);

            services.AddTransient(typeof(TConcrete), sp =>
            {
                var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                var cacheService = sp.GetRequiredService<ICacheService>();
                var cancellationTokenHelper = sp.GetRequiredService<ICancellationTokenHelper>();

                return (TConcrete)Activator.CreateInstance(typeof(TConcrete), httpClientfactory, httpContextAccessor, configurationAccessor, cacheService, cancellationTokenHelper, apiClientOption.apiKey);
            });

            return apiClientOption.apiKey;
        }
    }
}
