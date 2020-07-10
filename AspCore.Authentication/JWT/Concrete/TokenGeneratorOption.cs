using AspCore.Authentication.JWT.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Configuration;
using AspCore.Entities.EntityType;
using AspCore.Entities.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace AspCore.Authentication.JWT.Concrete
{
    public class TokenGeneratorOption : ConfigurationOption
    {
        public TokenGeneratorOption(IServiceCollection services) : base(services)
        {
        }

        public void AddTokenGenerator<TJWTInfo, TTokenGenerator, TTokenValidator>(Action<TokenConfigurationOption> option)
            where TJWTInfo : class, IJWTEntity, new()
            where TTokenGenerator : JwtGenerator<TJWTInfo>, ITokenGenerator<TJWTInfo>
            where TTokenValidator : JwtValidator<TJWTInfo>, ITokenValidator<TJWTInfo>
        {
            TokenConfigurationOption tokenConfigurationOption = new TokenConfigurationOption();
            option(tokenConfigurationOption);

            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

      

            services.AddSingleton(typeof(IJwtHandler), sp =>
            {
                return new JwtHandler(sp.GetRequiredService<IConfigurationAccessor>(), tokenConfigurationOption);

            });

            services.AddSingleton(typeof(ITokenGenerator<TJWTInfo>), sp =>
            {
                ITokenGenerator<TJWTInfo> implementation = null;

                if (!string.IsNullOrEmpty(tokenConfigurationOption.configurationKey))
                {
                    implementation = (TTokenGenerator)Activator.CreateInstance(typeof(TTokenGenerator), sp);
                }
                else
                {
                    implementation = (TTokenGenerator)Activator.CreateInstance(typeof(TTokenGenerator), sp);
                }

                return implementation;
            });

             services.AddSingleton(typeof(ITokenValidator<TJWTInfo>), sp =>
            {
                TTokenValidator implementation = null;

                if (!string.IsNullOrEmpty(tokenConfigurationOption.configurationKey))
                {
                    implementation = (TTokenValidator)Activator.CreateInstance(typeof(TTokenValidator), sp);
                }
                else
                {
                    implementation = (TTokenValidator)Activator.CreateInstance(typeof(TTokenValidator), sp);
                }

                return implementation;
            });


            AddAuthenticationSetting();

            services.ConfigureOptions<ConfigureJwtBearerOptions>();
        }

        public void AddActiveUserTokenGenerator<TImplementation, TImplementationValidator>(Action<TokenConfigurationOption> option)
            where TImplementation : JwtGenerator<ActiveUser>
            where TImplementationValidator : JwtValidator<ActiveUser>
        {
            TokenConfigurationOption tokenConfigurationOption = new TokenConfigurationOption();
            option(tokenConfigurationOption);

            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            services.AddSingleton(typeof(IJwtHandler), sp =>
            {
                return new JwtHandler(sp.GetRequiredService<IConfigurationAccessor>(), tokenConfigurationOption);

            });

            services.AddSingleton(typeof(ITokenGenerator<ActiveUser>), sp =>
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

            services.AddSingleton(typeof(ITokenValidator<ActiveUser>), sp =>
            {
                TImplementationValidator implementation = null;

                if (!string.IsNullOrEmpty(tokenConfigurationOption.configurationKey))
                {
                    implementation = (TImplementationValidator)Activator.CreateInstance(typeof(TImplementationValidator), sp);
                }
                else
                {
                    implementation = (TImplementationValidator)Activator.CreateInstance(typeof(TImplementationValidator), sp);
                }

                return implementation;
            });


            AddAuthenticationSetting();


            services.ConfigureOptions<ConfigureJwtBearerOptions>();
        }

        private void AddAuthenticationSetting()
        {
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(JwtBearerDefaults.AuthenticationScheme, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
            });


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();
        }
    }
}
