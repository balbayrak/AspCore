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
                services.AddSingleton(typeof(ITokenGenerator<TJWTInfo>), sp =>
                {
                    ITokenGenerator<TJWTInfo> implementation = (ITokenGenerator<TJWTInfo>)Activator.CreateInstance(typeof(TTokenGenerator), tokenConfigurationOption.configurationKey, null);
                    return implementation;
                });

                using (ServiceProvider serviceProvider = services.BuildServiceProvider())
                {
                    //configuration helper ile setting

                    IConfigurationAccessor configurationHelper = serviceProvider.GetRequiredService<IConfigurationAccessor>();
                    if (configurationHelper == null)
                    {
                        throw new Exception(ConfigurationHelperConstants.ErrorMessages.CONFIGURATION_HELPER_NOT_FOUND);
                    }

                    tokenConfigurationOption.tokenSettingOption = configurationHelper.GetValueByKey<TokenSettingOption>(tokenConfigurationOption.configurationKey);
                }
            }
            else
            {
                services.AddSingleton(typeof(IActiveUserTokenGenerator), sp =>
                {
                    TTokenGenerator implementation = (TTokenGenerator)Activator.CreateInstance(typeof(TTokenGenerator), null, tokenConfigurationOption.tokenSettingOption);

                    return implementation;
                });
            }

            if (tokenConfigurationOption.tokenSettingOption != null)
            {
                AddAuthenticationSetting(tokenConfigurationOption.tokenSettingOption);
            }
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
                services.AddSingleton(typeof(IActiveUserTokenGenerator), sp =>
                {
                    TImplementation implementation = (TImplementation)Activator.CreateInstance(typeof(TImplementation), tokenConfigurationOption.configurationKey, null);

                    return implementation;
                });

                using (ServiceProvider serviceProvider = services.BuildServiceProvider())
                {
                    //configuration helper ile setting

                    IConfigurationAccessor configurationHelper = serviceProvider.GetRequiredService<IConfigurationAccessor>();
                    if (configurationHelper == null)
                    {
                        throw new Exception(ConfigurationHelperConstants.ErrorMessages.CONFIGURATION_HELPER_NOT_FOUND);
                    }

                    tokenConfigurationOption.tokenSettingOption = configurationHelper.GetValueByKey<TokenSettingOption>(tokenConfigurationOption.configurationKey);
                }
            }
            else
            {
                services.AddSingleton(typeof(IActiveUserTokenGenerator), sp =>
                {
                    TImplementation implementation = (TImplementation)Activator.CreateInstance(typeof(TImplementation), null, tokenConfigurationOption.tokenSettingOption);

                    return implementation;
                });
            }

            if (tokenConfigurationOption.tokenSettingOption != null)
            {
                AddAuthenticationSetting(tokenConfigurationOption.tokenSettingOption);
            }
        }

        private void AddAuthenticationSetting(TokenSettingOption tokenSettingOption)
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
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = tokenSettingOption.Issuer ?? string.Empty,
                    ValidAudience = tokenSettingOption.Audience ?? string.Empty,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettingOption.SecurityKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = ctx =>
                    {
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add(ApiConstants.Api_Keys.TOKEN_EXPIRED_HEADER, "true");
                        }
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = ctx =>
                    {
                        Console.WriteLine("Exception:{0}", ctx.Request);
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
