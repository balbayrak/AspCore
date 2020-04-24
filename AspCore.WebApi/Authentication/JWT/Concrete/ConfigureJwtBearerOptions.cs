using AspCore.ConfigurationAccess.Abstract;
using AspCore.WebApi.Configuration.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.WebApi.Authentication.JWT.Concrete
{
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtAuthorizationOption jwtAuthorization;
        public ConfigureJwtBearerOptions(JwtAuthorizationOption jwtAuthorization)
        {
            this.jwtAuthorization = jwtAuthorization;
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            if (name == JwtBearerDefaults.AuthenticationScheme)
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtAuthorization._tokenSettingOption.Issuer ?? string.Empty,
                    ValidAudience = jwtAuthorization._tokenSettingOption.Audience ?? string.Empty,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthorization._tokenSettingOption.SecurityKey))
                };

                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = (context) =>
                    {
                        Console.WriteLine(context.Exception);
                        return Task.CompletedTask;
                    },

                    OnMessageReceived = (context) =>
                    {
                        return Task.CompletedTask;
                    },

                    OnTokenValidated = (context) =>
                    {
                        return Task.CompletedTask;
                    }
                };
            }
        }

        public void Configure(JwtBearerOptions options)
        {
            Configure(JwtBearerDefaults.AuthenticationScheme, options);
        }
    }
}
