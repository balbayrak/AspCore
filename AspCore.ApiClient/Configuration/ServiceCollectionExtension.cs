using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities;
using AspCore.ApiClient.Handlers;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Constants;
using AspCore.Storage.Abstract;
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

        public static string AddApiClient(this IServiceCollection services, Action<ApiClientOption> clientOption, bool injectImplementationType = false)
        {
            return AddClient<ApiClient<ApiClientConfiguration>>(services, clientOption, injectImplementationType);
        }

        public static string AddApiClient<TOption>(this IServiceCollection services, Action<ApiClientOption> clientOption, bool injectImplementationType = false)
            where TOption : class, IApiClientConfiguration, new()
        {
            return AddClient<ApiClient<TOption>>(services, clientOption, injectImplementationType);
        }

        public static string AddApiClient<T, TConcrete>(this IServiceCollection services, Action<ApiClientOption> apiClientOption, bool injectImplementationType = false)
        where T : IApiClient
        where TConcrete : ApiClient<ApiClientConfiguration>
        {
            return AddApiClient<T, TConcrete, ApiClientConfiguration>(services, apiClientOption, injectImplementationType);
        }

        public static string AddApiClient<T, TConcrete, TOption>(this IServiceCollection services, Action<ApiClientOption> clientOption, bool injectImplementationType = false)
           where T : IApiClient
           where TConcrete : ApiClient<TOption>, IApiClient
           where TOption : class, IApiClientConfiguration, new()

        {
            ApiClientOption apiClientOption = new ApiClientOption();
            clientOption(apiClientOption);

            AddClientConfiguration(services, apiClientOption);

            if (injectImplementationType)
            {
                services.AddTransient(typeof(TConcrete), sp =>
                {
                    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    return (TConcrete)Activator.CreateInstance(typeof(TConcrete), httpClientfactory, configurationAccessor, apiClientOption.apiKey);
                });
            }
            else
            {
                services.AddTransient(typeof(T), sp =>
                {
                    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    return (TConcrete)Activator.CreateInstance(typeof(TConcrete), httpClientfactory, configurationAccessor, apiClientOption.apiKey);
                });
            }


            return apiClientOption.apiKey;
        }

        public static string AddAuthenticatedApiClient(this IServiceCollection services, Action<AuthenticatedApiClientOption> clientOption, bool injectImplementationType = false)
        {
            return AddAuthenticatedApiClient<IApiClient, ApiClient<ApiClientConfiguration>, ApiClientConfiguration>(services, clientOption, injectImplementationType);
        }

        public static string AddAuthenticatedApiClient<TOption>(this IServiceCollection services, Action<AuthenticatedApiClientOption> clientOption, bool injectImplementationType = false)
           where TOption : class, IApiClientConfiguration, new()
        {
            return AddAuthenticatedApiClient<IApiClient, ApiClient<TOption>, TOption>(services, clientOption, injectImplementationType);
        }

        public static string AddAuthenticatedApiClient<TOption, TAuthenticationHandler>(this IServiceCollection services, Action<AuthenticatedApiClientOption> clientOption, bool injectImplementationType = false)
         where TOption : class, IApiClientConfiguration, new()
         where TAuthenticationHandler : AspCoreAuthenticationHandler<TOption>
        {
            return AddAuthenticatedApiClient<IApiClient, ApiClient<TOption>, TOption, TAuthenticationHandler>(services, clientOption, injectImplementationType);
        }

        public static string AddAuthenticatedApiClient<T, TConcrete, TOption>(this IServiceCollection services, Action<AuthenticatedApiClientOption> clientOption, bool injectImplementationType = false)
   where T : IApiClient
   where TConcrete : ApiClient<TOption>
   where TOption : class, IApiClientConfiguration, new()

        {
            AuthenticatedApiClientOption apiClientOption = new AuthenticatedApiClientOption();
            clientOption(apiClientOption);

            AddAuthenticatedClientConfiguration<TOption>(services, apiClientOption);

            if (injectImplementationType)
            {
                services.AddTransient(typeof(TConcrete), sp =>
                {
                    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    return (TConcrete)Activator.CreateInstance(typeof(TConcrete), httpClientfactory, configurationAccessor, apiClientOption.apiKey);
                });
            }
            else
            {
                services.AddTransient(typeof(T), sp =>
                {
                    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    return (TConcrete)Activator.CreateInstance(typeof(TConcrete), httpClientfactory, configurationAccessor, apiClientOption.apiKey);
                });
            }
            return apiClientOption.apiKey;
        }

        public static string AddAuthenticatedApiClient<T, TConcrete, TOption, TAuthenticationHandler>(this IServiceCollection services, Action<AuthenticatedApiClientOption> clientOption, bool injectImplementationType = false)
           where T : IApiClient
           where TConcrete : ApiClient<TOption>
           where TOption : class, IApiClientConfiguration, new()
           where TAuthenticationHandler : AspCoreAuthenticationHandler<TOption>

        {
            AuthenticatedApiClientOption apiClientOption = new AuthenticatedApiClientOption();
            clientOption(apiClientOption);

            AddAuthenticatedClientConfiguration<TOption, TAuthenticationHandler>(services, apiClientOption);

            if (injectImplementationType)
            {
                services.AddTransient(typeof(TConcrete), sp =>
                {
                    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    return (TConcrete)Activator.CreateInstance(typeof(TConcrete), httpClientfactory, configurationAccessor, apiClientOption.apiKey);
                });
            }
            else
            {
                services.AddTransient(typeof(T), sp =>
                {
                    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    return (TConcrete)Activator.CreateInstance(typeof(TConcrete), httpClientfactory, configurationAccessor, apiClientOption.apiKey);
                });
            }

            return apiClientOption.apiKey;
        }

        private static string AddClient<TConcrete>(IServiceCollection services, Action<ApiClientOption> clientOption, bool injectImplementationType = false)
            where TConcrete : class, IApiClient
        {
            ApiClientOption apiClientOption = new ApiClientOption();
            clientOption(apiClientOption);

            AddClientConfiguration(services, apiClientOption);

            if (injectImplementationType)
            {
                services.AddTransient(typeof(TConcrete), sp =>
                {
                    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    return (TConcrete)Activator.CreateInstance(typeof(TConcrete), httpClientfactory, configurationAccessor, apiClientOption.apiKey);
                });
            }
            else
            {
                services.AddTransient(typeof(IApiClient), sp =>
                {
                    var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    return (TConcrete)Activator.CreateInstance(typeof(TConcrete), httpClientfactory, configurationAccessor, apiClientOption.apiKey);
                });
            }

            return apiClientOption.apiKey;
        }

        private static IHttpClientBuilder AddClientConfiguration(IServiceCollection services, ApiClientOption apiClientOption)
        {
            return services.AddHttpClient(apiClientOption.apiKey, client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstants.Api_Keys.JSON_MEDIA_TYPE_QUALITY_HEADER));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(ApiConstants.Api_Keys.GZIP_COMPRESSION_STRING_WITH_QUALITY_HEADER));
            })
           .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMinutes(apiClientOption.timeout)))
           .AddPolicyHandler(GetRetryPolicy(apiClientOption.retryCount))
           .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(apiClientOption.circuitbreakerCount, TimeSpan.FromSeconds(30)))
           .ConfigurePrimaryHttpMessageHandler(() =>
           {
               return new HttpClientHandler()
               {
                   AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
               };
           })
           .AddHttpMessageHandler(sp =>
           {
               IHttpContextAccessor httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
               return new CorrelationIdHandler(httpContextAccessor);
           });

        }

        private static void AddAuthenticatedClientConfiguration<TOption, TAuthenticationHandler>(IServiceCollection services, AuthenticatedApiClientOption apiClientOption)
           where TOption : class, IApiClientConfiguration, new()
           where TAuthenticationHandler : AspCoreAuthenticationHandler<TOption>
        {
            AddClientConfiguration(services, apiClientOption).AddHttpMessageHandler(sp =>
              {
                  return (TAuthenticationHandler)Activator.CreateInstance(typeof(TAuthenticationHandler), sp, apiClientOption.apiKey);
              });
        }

        private static void AddAuthenticatedClientConfiguration<TOption>(IServiceCollection services, AuthenticatedApiClientOption apiClientOption)
          where TOption : class, IApiClientConfiguration, new()
        {
            AddClientConfiguration(services, apiClientOption).AddHttpMessageHandler(sp =>
            {
                if (apiClientOption.authenticationHandler == EnumAuthenticationHandler.AuthService)
                {
                    return new AuthServiceBasedAuthenticationHandler<ApiClientConfiguration>(sp, apiClientOption.apiKey);
                }
                else if (apiClientOption.authenticationHandler == EnumAuthenticationHandler.Cache)
                {
                    var cacheService = sp.GetRequiredService<ICacheService>();
                    return new CacheBasedAuthenticationHandler<TOption>(sp, apiClientOption.apiKey);
                }
                return null;
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
    }
}
