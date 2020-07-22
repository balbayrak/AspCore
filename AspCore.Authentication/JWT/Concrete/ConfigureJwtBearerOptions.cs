﻿using AspCore.Authentication.JWT.Abstract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.Authentication.JWT.Concrete
{
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private IJwtHandler _jwtHandler;
        public ConfigureJwtBearerOptions(IJwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            if (name == JwtBearerDefaults.AuthenticationScheme)
            {

                options.TokenValidationParameters = _jwtHandler.Parameters;
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
                    },
                    
                };
            }
        }
        

        public void Configure(JwtBearerOptions options)
        {
            Configure(JwtBearerDefaults.AuthenticationScheme, options);
        }
    }
}
