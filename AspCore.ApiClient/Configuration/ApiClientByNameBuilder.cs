using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities;
using AspCore.ApiClient.Entities.Abstract;
using AspCore.Caching.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Dependency.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;


namespace AspCore.ApiClient.Configuration
{
    public class ApiClientByNameBuilder
    {
        private readonly IServiceCollection _services;

        private readonly IDictionary<string, Type> _registrations = new Dictionary<string, Type>();

        public ApiClientByNameBuilder(IServiceCollection services)
        {
            _services = services;
        }


        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "IApiClientConfiguration" type.
        /// </summary>
        /// <typeparam name="TOption">IApiClientConfiguration type</typeparam>
        /// <param name="apiKey">configuration key</param>
        /// <param name="timeout">HttpClient timeout value(minutes)</param>
        /// <param name="retryCount">Retry Count for unsuccessful request</param>
        /// <param name="circuitbreakerCount">Cicuit after unsuccessful request count</param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddApiClient<TOption>(string apiKey, int timeout = 2, int retryCount = 3, int circuitbreakerCount = 5)
       where TOption : class, IApiClientConfiguration, new()
        {
            return AddClient<JWTAuthenticatedApiClient<ApiClientConfiguration>>(apiKey, timeout, retryCount, circuitbreakerCount);
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <param name="apiKey">configuration key</param>
        /// <param name="timeout">HttpClient timeout value(minutes)</param>
        /// <param name="retryCount">Retry Count for unsuccessful request</param>
        /// <param name="circuitbreakerCount">Cicuit after unsuccessful request count</param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddApiClient(string apiKey, int timeout = 2, int retryCount = 3, int circuitbreakerCount = 5)
        {
            return AddClient<JWTAuthenticatedApiClient<ApiClientConfiguration>>(apiKey, timeout, retryCount, circuitbreakerCount);
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "IApiClientConfiguration" type.
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="apiKey">configuration key</param>
        /// <param name="timeout">HttpClient timeout value(minutes)</param>
        /// <param name="retryCount">Retry Count for unsuccessful request</param>
        /// <param name="circuitbreakerCount">Cicuit after unsuccessful request count</param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddBearerAuthenticatedClient<TOption>(string apiKey, int timeout = 2, int retryCount = 3, int circuitbreakerCount = 5)
          where TOption : class, IApiClientConfiguration, new()
        {
            return AddClient<JWTAuthenticatedApiClient<ApiClientConfiguration>>(apiKey, timeout, retryCount, circuitbreakerCount);
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <param name="apiKey">configuration key</param>
        /// <param name="timeout">HttpClient timeout value(minutes)</param>
        /// <param name="retryCount">Retry Count for unsuccessful request</param>
        /// <param name="circuitbreakerCount">Cicuit after unsuccessful request count</param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddBearerAuthenticatedClient(string apiKey, int timeout = 2, int retryCount = 3, int circuitbreakerCount = 5)
        {
            return AddClient<JWTAuthenticatedApiClient<ApiClientConfiguration>>(apiKey, timeout, retryCount, circuitbreakerCount);
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "IApiClientConfiguration" type.
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="apiKey">configuration key</param>
        /// <param name="timeout">HttpClient timeout value(minutes)</param>
        /// <param name="retryCount">Retry Count for unsuccessful request</param>
        /// <param name="circuitbreakerCount">Cicuit after unsuccessful request count</param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddJWTAuthenticatedClient<TOption>(string apiKey, int timeout = 2, int retryCount = 3, int circuitbreakerCount = 5)
        where TOption : class, IApiClientConfiguration, new()
        {
            return AddClient<JWTAuthenticatedApiClient<ApiClientConfiguration>>(apiKey, timeout, retryCount, circuitbreakerCount);
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <param name="apiKey">configuration key</param>
        /// <param name="timeout">HttpClient timeout value(minutes)</param>
        /// <param name="retryCount">Retry Count for unsuccessful request</param>
        /// <param name="circuitbreakerCount">Cicuit after unsuccessful request count</param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddJWTAuthenticatedClient(string apiKey,  int timeout = 2, int retryCount = 3, int circuitbreakerCount = 5)
        {
            return AddClient<JWTAuthenticatedApiClient<ApiClientConfiguration>>(apiKey, timeout, retryCount, circuitbreakerCount);
        }

        private ApiClientByNameBuilder AddClient<T>(string apiKey, int timeout = 2, int retryCount = 3,int circuitbreakerCount = 5)
            where T : class, IApiClient
        {
            _services.AddHttpClient(apiKey, client =>
             {
                 client.DefaultRequestHeaders.Accept.Clear();
                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstants.Api_Keys.JSON_MEDIA_TYPE_QUALITY_HEADER));
                 client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(ApiConstants.Api_Keys.GZIP_COMPRESSION_STRING_WITH_QUALITY_HEADER));

             })
             .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMinutes(timeout)))
             .AddPolicyHandler(GetRetryPolicy(retryCount))
             .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(circuitbreakerCount, TimeSpan.FromSeconds(30)))
             .AddHeaderPropagation().ConfigurePrimaryHttpMessageHandler(() =>
             {
                 return new HttpClientHandler()
                 {
                     AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                 };
             });



            _services.AddTransient(typeof(T), sp =>
            {
                var httpClientfactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                var cacheService = sp.GetRequiredService<ICacheService>();
                var cancellationTokenHelper = sp.GetRequiredService<ICancellationTokenHelper>();

                return (T)Activator.CreateInstance(typeof(T), httpClientfactory, httpContextAccessor, configurationAccessor, cacheService, cancellationTokenHelper, apiKey);
            });

            _registrations.Add(apiKey, typeof(T));

            return this;
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
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

        public void Build()
        {
            var registrations = _registrations;
            _services.AddTransient<IServiceByNameFactory<IAuthenticatedApiClient>>(s => new ServiceByNameFactory<IAuthenticatedApiClient>(registrations));
        }
    }
}
