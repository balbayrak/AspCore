using AspCore.Entities.Authentication;
using System;

namespace AspCore.WebApi.Configuration.Options
{
    public class JWTAuthorizationFilterOption : AuthorizationFilterOption
    {
        public Type authenticationProviderType { get; set; }

    }
}
