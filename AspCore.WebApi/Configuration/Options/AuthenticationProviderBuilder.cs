﻿using AspCore.Authentication.JWT.Concrete;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Configuration;
using AspCore.WebApi.Authentication.Providers.Abstract;
using AspCore.WebApi.Authentication.Providers.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.Entities.User;

namespace AspCore.WebApi.Configuration.Options
{
    public class AuthenticationProviderBuilder : ConfigurationOption
    {
        public AuthenticationProviderBuilder(IServiceCollection services) : base(services)
        {
        }

        public TokenGeneratorOption AddAuthenticationProvider<TInput, TOutput, TAuthenticationProvider>()
            where TAuthenticationProvider : class, IApiAuthenticationProvider<TInput, TOutput>
            where TInput : AuthenticationInfo
            where TOutput : class, new()
        {
            services.AddSingleton<IApiAuthenticationProvider<TInput, TOutput>, TAuthenticationProvider>();
            return new TokenGeneratorOption(services);
        }


      
        public TokenGeneratorOption AddAppSettingAuthenticationProvider<TInput, TOutput, TOption, TAuthenticationProvider>(Action<AppSettingsAuthProviderOption<TOption>> option)
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

            return new TokenGeneratorOption(services);
        }

        public TokenGeneratorOption AddAuthenticationProvider(Action<ServicesByNameBuilder<IActiveUserAuthenticationProvider>> builder)
        {
            ServicesByNameBuilder<IActiveUserAuthenticationProvider> servicesByNameBuilder = new ServicesByNameBuilder<IActiveUserAuthenticationProvider>(services, ServiceLifetime.Transient);
            builder(servicesByNameBuilder);

            return new TokenGeneratorOption(services);
        }

        public TokenGeneratorOption AddAuthenticationProvider<T, TInput, TOutput>(Action<ServicesByNameBuilder<T>> builder)
        where T: IApiAuthenticationProvider<TInput, TOutput>
        where TInput : AuthenticationInfo
        where TOutput : class, new()
        {
            ServicesByNameBuilder<T> servicesByNameBuilder = new ServicesByNameBuilder<T>(services, ServiceLifetime.Transient);
            builder(servicesByNameBuilder);
            return new TokenGeneratorOption(services);
        }
    }
}
