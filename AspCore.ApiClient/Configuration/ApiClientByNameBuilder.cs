using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities;
using AspCore.ApiClient.Handlers;
using AspCore.Dependency.Abstract;
using AspCore.Dependency.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;


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
            string apiKey = _services.AddApiClient<TOption>(clientOption,true);

            _registrations.Add(apiKey, typeof(ApiClient<TOption>));

            return this;
        }

        public ApiClientByNameBuilder AddApiClient<T, TConcrete, TOption>(Action<ApiClientOption> clientOption)
           where T : IApiClient
           where TConcrete : ApiClient<TOption>, IApiClient
           where TOption : class, IApiClientConfiguration, new()
        {
            string apiKey = _services.AddApiClient<T, TConcrete, TOption>(clientOption,true);

            _registrations.Add(apiKey, typeof(ApiClient<TOption>));

            return this;
        }

        public ApiClientByNameBuilder AddApiClient(Action<ApiClientOption> clientOption)
        {
            string apiKey = _services.AddApiClient<ApiClientConfiguration>(clientOption,true);

            _registrations.Add(apiKey, typeof(ApiClient<ApiClientConfiguration>));

            return this;
        }

        public ApiClientByNameBuilder AddAuthenticatedApiClient<TOption>(Action<AuthenticatedApiClientOption> clientOption)
          where TOption : class, IApiClientConfiguration, new()
        {
            string apiKey = _services.AddAuthenticatedApiClient<TOption>(clientOption,true);

            _registrations.Add(apiKey, typeof(ApiClient<TOption>));

            return this;
        }

        public ApiClientByNameBuilder AddAuthenticatedApiClient<T, TConcrete, TOption>(Action<AuthenticatedApiClientOption> clientOption)
            where T : IApiClient
            where TConcrete : ApiClient<TOption>, IApiClient
            where TOption : class, IApiClientConfiguration, new()
        {
            string apiKey = _services.AddAuthenticatedApiClient<T, TConcrete, TOption>(clientOption,true);

            _registrations.Add(apiKey, typeof(ApiClient<TOption>));

            return this;
        }
        
        public ApiClientByNameBuilder AddAuthenticatedApiClient<T, TConcrete, TOption, TAuthenticationHandler>(Action<AuthenticatedApiClientOption> clientOption)
           where T : IApiClient
           where TConcrete : ApiClient<TOption>, IApiClient
           where TOption : class, IApiClientConfiguration, new()
             where TAuthenticationHandler : AspCoreAuthenticationHandler<TOption>
        {
            string apiKey = _services.AddAuthenticatedApiClient<T, TConcrete, TOption, TAuthenticationHandler>(clientOption,true);

            _registrations.Add(apiKey, typeof(ApiClient<TOption>));

            return this;
        }

        public ApiClientByNameBuilder AddAuthenticatedApiClient<TOption, TAuthenticationHandler>(Action<AuthenticatedApiClientOption> clientOption)
            where TOption : class, IApiClientConfiguration, new()
            where TAuthenticationHandler : AspCoreAuthenticationHandler<TOption>
        {
            string apiKey = _services.AddAuthenticatedApiClient<TOption, TAuthenticationHandler>(clientOption,true);

            _registrations.Add(apiKey, typeof(ApiClient<TOption>));

            return this;
        }

        public ApiClientByNameBuilder AddAuthenticatedApiClient(Action<AuthenticatedApiClientOption> clientOption)
        {
            string apiKey = _services.AddAuthenticatedApiClient(clientOption,true);

            _registrations.Add(apiKey, typeof(ApiClient<ApiClientConfiguration>));

            return this;
        }

        public void Build()
        {
            var registrations = _registrations;
            _services.AddTransient<IServiceByNameFactory<IApiClient>>(s => new ServiceByNameFactory<IApiClient>(registrations));
        }
    }
}
