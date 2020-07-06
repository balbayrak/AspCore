using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities;
using AspCore.ApiClient.Entities.Abstract;
using AspCore.Dependency.Abstract;
using AspCore.Dependency.Concrete;
using Microsoft.AspNetCore.Http;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Caching.Abstract;
using Microsoft.Extensions.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using AspCore.Entities.Constants;
using System.Net;
using System.Threading;
using AspCore.Extension;

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
        /// <typeparam name="TOption"></typeparam>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddApiClient<TOption>(string apiKey)
       where TOption : class, IApiClientConfiguration, new()
        {
            return AddClient<ApiClient<TOption>>(apiKey);
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddApiClient(string apiKey)
        {
            return AddClient<ApiClient<ApiClientConfiguration>>(apiKey);
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "IApiClientConfiguration" type.
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddBearerAuthenticatedClient<TOption>(string apiKey)
          where TOption : class, IApiClientConfiguration, new()
        {
            return AddClient<BearerAuthenticatedApiClient<TOption>>(apiKey);
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddBearerAuthenticatedClient(string apiKey)
        {
            return AddClient<BearerAuthenticatedApiClient<ApiClientConfiguration>>(apiKey);
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "IApiClientConfiguration" type.
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddJWTAuthenticatedClient<TOption>(string apiKey)
        where TOption : class, IApiClientConfiguration, new()
        {
            return AddClient<JWTAuthenticatedApiClient<TOption>>(apiKey);
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddJWTAuthenticatedClient(string apiKey)
        {

            return AddClient<JWTAuthenticatedApiClient<ApiClientConfiguration>>(apiKey);

        }

        private ApiClientByNameBuilder AddClient<T>(string apiKey)
            where T : class, IApiClient
        {
            _services.AddHttpClient(apiKey,client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstants.Api_Keys.JSON_MEDIA_TYPE_QUALITY_HEADER));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(ApiConstants.Api_Keys.GZIP_COMPRESSION_STRING_WITH_QUALITY_HEADER));

            }).AddHeaderPropagation().ConfigurePrimaryHttpMessageHandler(() =>
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
        public void Build()
        {
            var registrations = _registrations;
            _services.AddTransient<IServiceByNameFactory<IAuthenticatedApiClient>>(s => new ServiceByNameFactory<IAuthenticatedApiClient>(registrations));
        }
    }
}
