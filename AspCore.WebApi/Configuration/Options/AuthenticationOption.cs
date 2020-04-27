using AspCore.ConfigurationAccess.Abstract;
using AspCore.ConfigurationAccess.Concrete;
using AspCore.Entities.Configuration;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.User;
using AspCore.WebApi.Authentication.Abstract;
using AspCore.WebApi.Authentication.JWT.Concrete;
using AspCore.WebApi.Security.Abstract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.WebApi.Configuration.Options
{
    public class AuthenticationOption : ConfigurationOption
    {
        public AuthenticationOption(IServiceCollection services) : base(services)
        {
        }

        public void AddTokenGenerator<TJWTInfo, TTokenGenerator>(Action<TokenConfigurationOption> option)
            where TJWTInfo : class, IJWTEntity, new()
            where TTokenGenerator : JwtGenerator<TJWTInfo>, ITokenGenerator<TJWTInfo>
        {
            TokenConfigurationOption tokenConfigurationOption = new TokenConfigurationOption();
            option(tokenConfigurationOption);

            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            if (!string.IsNullOrEmpty(tokenConfigurationOption.configurationKey))
            {
                services.AddSingleton(typeof(JwtAuthorizationOption), sp =>
                {
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    return new JwtAuthorizationOption(configurationAccessor, tokenConfigurationOption.configurationKey);
                });
            }

            services.AddSingleton(typeof(ITokenGenerator<TJWTInfo>), sp =>
            {
                ITokenGenerator<TJWTInfo> implementation = null;

                if (!string.IsNullOrEmpty(tokenConfigurationOption.configurationKey))
                {
                    implementation = (ITokenGenerator<TJWTInfo>)Activator.CreateInstance(typeof(TTokenGenerator), sp, tokenConfigurationOption.configurationKey, null);
                }
                else
                {
                    implementation = (TTokenGenerator)Activator.CreateInstance(typeof(TTokenGenerator), sp, null, tokenConfigurationOption.tokenSettingOption);
                }

                return implementation;
            });

           
                AddAuthenticationSetting();
            services.ConfigureOptions<ConfigureJwtBearerOptions>();
        }

        public void AddActiveUserTokenGenerator<TImplementation>(Action<TokenConfigurationOption> option)
            where TImplementation : JwtGenerator<ActiveUser>
        {
            TokenConfigurationOption tokenConfigurationOption = new TokenConfigurationOption();
            option(tokenConfigurationOption);

            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            if (!string.IsNullOrEmpty(tokenConfigurationOption.configurationKey))
            {
                services.AddSingleton(typeof(JwtAuthorizationOption), sp =>
                {
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    return new JwtAuthorizationOption(configurationAccessor, tokenConfigurationOption.configurationKey);
                });
            }

            services.AddSingleton(typeof(IActiveUserTokenGenerator), sp =>
            {
                TImplementation implementation = null;

                if (!string.IsNullOrEmpty(tokenConfigurationOption.configurationKey))
                {
                    implementation = (TImplementation)Activator.CreateInstance(typeof(TImplementation), sp, tokenConfigurationOption.configurationKey, null);
                }
                else
                {
                    implementation = (TImplementation)Activator.CreateInstance(typeof(TImplementation), sp, null, tokenConfigurationOption.tokenSettingOption);
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
