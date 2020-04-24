using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Configuration;
using AspCore.WebApi.Authentication.Providers.Abstract;
using AspCore.WebApi.Authentication.Providers.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.WebApi.Configuration.Options
{
    public class AuthenticationProviderBuilder : ConfigurationOption
    {
        public AuthenticationProviderBuilder(IServiceCollection services) : base(services)
        {
        }

        public AuthenticationOption AddAuthenticationProvider<TInput, TOutput, TAuthenticationProvider>()
            where TAuthenticationProvider : class, IApiAuthenticationProvider<TInput, TOutput>
            where TInput : AuthenticationInfo
            where TOutput : class, new()
        {
            services.AddSingleton<IApiAuthenticationProvider<TInput, TOutput>, TAuthenticationProvider>();
            return new AuthenticationOption(services);
        }

        public AuthenticationOption AddAppSettingAuthenticationProvider<TInput, TOutput, TOption, TAuthenticationProvider>(Action<AppSettingsAuthProviderOption<TOption>> option)
         where TAuthenticationProvider : AppSettingsAuthenticationProvider<TInput, TOutput, TOption>
         where TInput : AuthenticationInfo
         where TOutput : class, new()
         where TOption : class, IConfigurationEntity, new()
        {
            AppSettingsAuthProviderOption<TOption> appSettingsAuthProviderOption = new AppSettingsAuthProviderOption<TOption>();
            option(appSettingsAuthProviderOption);

            if (!string.IsNullOrEmpty(appSettingsAuthProviderOption.configurationKey))
            {
                services.AddSingleton(typeof(IApiAuthenticationProvider<TInput, TOutput>), sp =>
                {
                    IApiAuthenticationProvider<TInput, TOutput> implementation = (IApiAuthenticationProvider<TInput, TOutput>)Activator.CreateInstance(typeof(TAuthenticationProvider), sp,appSettingsAuthProviderOption.configurationKey, null);
                    return implementation;
                });

            }
            else if (appSettingsAuthProviderOption.option != null)
            {
                services.AddSingleton(typeof(IApiAuthenticationProvider<TInput, TOutput>), sp =>
                {
                    IApiAuthenticationProvider<TInput, TOutput> implementation = (IApiAuthenticationProvider<TInput, TOutput>)Activator.CreateInstance(typeof(TAuthenticationProvider),sp, null, appSettingsAuthProviderOption.option);
                    return implementation;
                });
            }

            return new AuthenticationOption(services);
        }

        public AuthenticationOption AddAuthenticationProvider(Action<ServicesByNameBuilder<IActiveUserAuthenticationProvider>> builder)
        {
            ServicesByNameBuilder<IActiveUserAuthenticationProvider> servicesByNameBuilder = new ServicesByNameBuilder<IActiveUserAuthenticationProvider>(services, ServiceLifetime.Transient);
            builder(servicesByNameBuilder);

            return new AuthenticationOption(services);
        }

    }
}
