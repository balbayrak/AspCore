using System;
using AspCore.Authentication.Concrete;

namespace AspCore.WebApi.Configuration.Options
{
    public class JWTAuthorizationFilterOption : AuthorizationFilterOption
    {
        public Type authenticationProviderType { get; set; }

    }
}
