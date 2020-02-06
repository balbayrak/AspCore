using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.Authentication.Abstract;
using AspCore.Authentication.Concrete;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Configuration;

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
            ServicesByNameBuilder<TAuthenticationProvider> servicesByNameBuilder = new ServicesByNameBuilder<TAuthenticationProvider>(_services, ServiceLifetime.Transient);
            builder(servicesByNameBuilder);

            return new StorageOptionConfiguration(_services);
        }

        public StorageOptionConfiguration AddAuthenticationProvider(Action<ServicesByNameBuilder<IWebAuthenticationProvider<AuthenticationInfo>>> builder)
        {
            ServicesByNameBuilder<IWebAuthenticationProvider<AuthenticationInfo>> servicesByNameBuilder = new ServicesByNameBuilder<IWebAuthenticationProvider<AuthenticationInfo>>(_services, ServiceLifetime.Transient);
            builder(servicesByNameBuilder);

            return new StorageOptionConfiguration(_services);
        }
    }
}
