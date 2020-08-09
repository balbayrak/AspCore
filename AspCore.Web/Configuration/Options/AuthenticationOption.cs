using AspCore.Entities.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Principal;

namespace AspCore.Web.Configuration.Options
{
    public class AuthenticationOption : ConfigurationOption
    {
        public AuthenticationOption(IServiceCollection services) : base(services)
        {
        }

        public StorageOptionConfiguration AddCookieAuthentication(Action<CookieAuthenticationBuilder> option)
        {
            CookieAuthenticationBuilder cookieAuthenticationBuilder = new CookieAuthenticationBuilder(services);
            option(cookieAuthenticationBuilder);

            return new StorageOptionConfiguration(services);
        }
    }
}
