using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities;
using AspCore.ApiClient.Entities.Abstract;
using AspCore.Dependency.Abstract;
using AspCore.Dependency.Concrete;

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
            _services.AddTransient(typeof(ApiClient<TOption>), sp =>
            {
                return new ApiClient<TOption>(apiKey);
            });

            _registrations.Add(apiKey, typeof(ApiClient<TOption>));

            return this;
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddApiClient(string apiKey)
        {
            _services.AddTransient(typeof(ApiClient<ApiClientConfiguration>), sp =>
            {
                return new ApiClient<ApiClientConfiguration>(apiKey);
            });

            _registrations.Add(apiKey, typeof(ApiClient<ApiClientConfiguration>));

            return this;
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
            _services.AddTransient(typeof(BearerAuthenticatedApiClient<TOption>), sp =>
            {
                return new BearerAuthenticatedApiClient<TOption>(apiKey);
            });

            _registrations.Add(apiKey, typeof(BearerAuthenticatedApiClient<TOption>));

            return this;
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddBearerAuthenticatedClient(string apiKey)
        {

            _services.AddTransient(typeof(BearerAuthenticatedApiClient<ApiClientConfiguration>), sp =>
            {
                return new BearerAuthenticatedApiClient<ApiClientConfiguration>(apiKey);
            });

            _registrations.Add(apiKey, typeof(BearerAuthenticatedApiClient<ApiClientConfiguration>));

            return this;
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

            _services.AddTransient(typeof(JWTAuthenticatedApiClient<TOption>), sp =>
            {
                return new JWTAuthenticatedApiClient<TOption>(apiKey);
            });

            _registrations.Add(apiKey, typeof(JWTAuthenticatedApiClient<TOption>));

            return this;
        }

        /// <summary>
        /// Configuration must be defined in configuration file or configuration storage. Configuration must be "ApiClientConfiguration" type.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public ApiClientByNameBuilder AddJWTAuthenticatedClient(string apiKey)
        {
            _services.AddTransient(typeof(JWTAuthenticatedApiClient<ApiClientConfiguration>), sp =>
            {
                return new JWTAuthenticatedApiClient<ApiClientConfiguration>(apiKey);
            });

            _registrations.Add(apiKey, typeof(JWTAuthenticatedApiClient<ApiClientConfiguration>));

            return this;
        }

        public void Build()
        {
            var registrations = _registrations;
            _services.AddTransient<IServiceByNameFactory<IAuthenticatedApiClient>>(s => new ServiceByNameFactory<IAuthenticatedApiClient>(registrations));
        }
    }
}
