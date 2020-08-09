using AspCore.Authentication.JWT.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Configuration;
using AspCore.Entities.EntityType;
using AspCore.Entities.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace AspCore.Authentication.JWT.Concrete
{
    public class TokenValidatorOption : ConfigurationOption
    {
        public TokenValidatorOption(IServiceCollection services) : base(services)
        {
        }

        public void AddTokenValidator<TJWTInfo, TTokenValidator>(Action<TokenConfigurationOption> option)
            where TJWTInfo : class, IJWTEntity, new()
            where TTokenValidator : JwtValidator<TJWTInfo>, ITokenValidator<TJWTInfo>
        {
            TokenConfigurationOption tokenConfigurationOption = new TokenConfigurationOption();
            option(tokenConfigurationOption);

            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            //AddTokenConfigurationOption(tokenConfigurationOption);
            services.AddSingleton(typeof(IJwtHandler), sp =>
            {
                return new JwtHandler(sp.GetRequiredService<IConfigurationAccessor>(), tokenConfigurationOption);
            });

            services.AddSingleton(typeof(ITokenValidator<TJWTInfo>), sp =>
            {
                ITokenValidator<TJWTInfo> implementation = null;

                if (!string.IsNullOrEmpty(tokenConfigurationOption.configurationKey))
                {
                    implementation = (ITokenValidator<TJWTInfo>)Activator.CreateInstance(typeof(TTokenValidator), sp);
                }
                else
                {
                    implementation = (TTokenValidator)Activator.CreateInstance(typeof(TTokenValidator), sp);
                }

                return implementation;
            });

        }

        public void AddActiveUserTokenValidator<TImplementation>(Action<TokenConfigurationOption> option)
            where TImplementation : JwtValidator<ActiveUser>
        {
            TokenConfigurationOption tokenConfigurationOption = new TokenConfigurationOption();
            option(tokenConfigurationOption);

            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }


           // AddTokenConfigurationOption(tokenConfigurationOption);

                   services.AddSingleton(typeof(IJwtHandler), sp =>
            {
                return new JwtHandler(sp.GetRequiredService<IConfigurationAccessor>(), tokenConfigurationOption);

            });

            services.AddSingleton(typeof(ITokenValidator<ActiveUser>), sp =>
            {
                TImplementation implementation = null;

                if (!string.IsNullOrEmpty(tokenConfigurationOption.configurationKey))
                {
                    implementation = (TImplementation)Activator.CreateInstance(typeof(TImplementation), sp);
                }
                else
                {
                    implementation = (TImplementation)Activator.CreateInstance(typeof(TImplementation), sp);
                }

                return implementation;
            });
        }

        private void AddTokenConfigurationOption(TokenConfigurationOption tokenValidatorConfiguration)
        {
            if (tokenValidatorConfiguration.tokenOption != null)
            {
                services.AddSingleton(tokenValidatorConfiguration.tokenOption);
            }

            if (!string.IsNullOrEmpty(tokenValidatorConfiguration.configurationKey))
            {
                services.AddSingleton(typeof(TokenOption), sp =>
                {
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    return configurationAccessor.GetValueByKey<TokenOption>(tokenValidatorConfiguration.configurationKey);
                });
            }
        }

    }
}
