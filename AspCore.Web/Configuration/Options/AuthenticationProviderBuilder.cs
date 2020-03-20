using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Configuration;
using AspCore.Web.Authentication.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Web.Configuration.Options
{
    public class AuthenticationProviderBuilder : ConfigurationOption
    {
        public AuthenticationProviderBuilder(IServiceCollection services) : base(services)
        {
        }

        public StorageOptionConfiguration AddCustomAuthenticationProvider<TInput, TAuthenticationProvider>(Action<ServicesByNameBuilder<TAuthenticationProvider>> builder)
        where TAuthenticationProvider : IWebAuthenticationProvider<TInput>
        where TInput : AuthenticationInfo
        {
            ServicesByNameBuilder<TAuthenticationProvider> servicesByNameBuilder = new ServicesByNameBuilder<TAuthenticationProvider>(services, ServiceLifetime.Transient);
            builder(servicesByNameBuilder);

            return new StorageOptionConfiguration(services);
        }

        public StorageOptionConfiguration AddAuthenticationProvider(Action<ServicesByNameBuilder<IWebAuthenticationProvider<AuthenticationInfo>>> builder)
        {
            ServicesByNameBuilder<IWebAuthenticationProvider<AuthenticationInfo>> servicesByNameBuilder = new ServicesByNameBuilder<IWebAuthenticationProvider<AuthenticationInfo>>(services, ServiceLifetime.Transient);
            builder(servicesByNameBuilder);

            return new StorageOptionConfiguration(services);
        }
    }
}
