using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Text;

namespace AspCore.Web.Configuration.Options
{
    public class CookieAuthenticationBuilder : ConfigurationOption
    {
        public CookieAuthenticationBuilder(IServiceCollection services) : base(services)
        {

        }

        /// <summary>
        /// Configuration must be AuthCookieOption type
        /// </summary>
        /// <param name="configurationKey"></param>
        /// <returns></returns>
        public AuthenticationProviderBuilder AddCookieSetting(string configurationKey)
        {
            AuthenticationSetting();

            services.AddSingleton<CookieConfigurationBuilder>(sp =>
            {
                var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                return new CookieConfigurationBuilder(configurationAccessor, configurationKey);

            });

            return new AuthenticationProviderBuilder(services);
        }

        public AuthenticationProviderBuilder AddCookieSetting([NotNull] Action<AuthCookieOption> option)
        {
            AuthCookieOption cookieOption = new AuthCookieOption();
            option(cookieOption);

            AuthenticationSetting();

            services.AddSingleton<CookieConfigurationBuilder>(sp =>
            {
                return new CookieConfigurationBuilder(cookieOption);
            });

            return new AuthenticationProviderBuilder(services);
        }

        private void AuthenticationSetting()
        {

            services.AddAuthentication(options =>
            {
                // these must be set other ASP.NET Core will throw exception that no
                // default authentication scheme or default challenge scheme is set.
                options.DefaultAuthenticateScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                        CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme =
                       CookieAuthenticationDefaults.AuthenticationScheme;
            })
           .AddCookie();

            services.AddTransient<IPrincipal>(
       provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

            services.AddAuthorization();

            services.ConfigureOptions<ConfigureCookieOption>();
        }
    }
}
