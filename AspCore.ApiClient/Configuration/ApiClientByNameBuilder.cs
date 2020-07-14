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

        public ApiClientByNameBuilder AddApiClient<TOption>(Action<ApiClientOption> clientOption)
       where TOption : class, IApiClientConfiguration, new()
        {
            string apiKey = _services.AddApiClient(clientOption);

           _registrations.Add(apiKey, typeof(ApiClient<TOption>));

            return this;
        }

        public ApiClientByNameBuilder AddApiClient(Action<ApiClientOption> clientOption)
        {
            string apiKey = _services.AddApiClient(clientOption);

            _registrations.Add(apiKey, typeof(ApiClient<ApiClientConfiguration>));

            return this;
        }

        public ApiClientByNameBuilder AddBearerAuthenticatedClient<TOption>(Action<ApiClientOption> clientOption)
          where TOption : class, IApiClientConfiguration, new()
        {
            string apiKey = _services.AddBearerAuthenticatedClient<TOption>(clientOption);

            _registrations.Add(apiKey, typeof(BearerAuthenticatedApiClient<TOption>));

            return this;
        }

        public ApiClientByNameBuilder AddBearerAuthenticatedClient(Action<ApiClientOption> clientOption)
        {
            string apiKey = _services.AddBearerAuthenticatedClient(clientOption);

            _registrations.Add(apiKey, typeof(BearerAuthenticatedApiClient<ApiClientConfiguration>));

            return this;
        }

        public ApiClientByNameBuilder AddJWTAuthenticatedClient<TOption>(Action<ApiClientOption> clientOption)
        where TOption : class, IApiClientConfiguration, new()
        {
            string apiKey = _services.AddJWTAuthenticatedClient<TOption>(clientOption);

            _registrations.Add(apiKey, typeof(JWTAuthenticatedApiClient<TOption>));

            return this;
        }

        public ApiClientByNameBuilder AddJWTAuthenticatedClient(Action<ApiClientOption> clientOption)
        {
            string apiKey = _services.AddJWTAuthenticatedClient(clientOption);
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
