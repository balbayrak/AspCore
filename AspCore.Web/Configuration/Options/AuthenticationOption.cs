using AspCore.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Web.Configuration.Options
{
    public class AuthenticationOption : ConfigurationOption
    {
        public AuthenticationOption(IServiceCollection services) : base(services)
        {
        }

        public CacheOptionConfiguration AddJWTAuthentication(Action<AuthenticationProviderBuilder> option)
        {
            AuthenticationProviderBuilder authenticationProviderBuilder = new AuthenticationProviderBuilder(services);
            option(authenticationProviderBuilder);
            return new CacheOptionConfiguration(services);
        }
    }
}
