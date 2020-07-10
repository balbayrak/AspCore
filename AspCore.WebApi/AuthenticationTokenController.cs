using AspCore.Authentication.JWT.Abstract;
using AspCore.Entities.Authentication;
using AspCore.Entities.User;
using AspCore.WebApi.Authentication.Providers.Abstract;
using System;

namespace AspCore.WebApi
{
    public class AuthenticationTokenController : BaseJWTAuthenticationController<IActiveUserAuthenticationProvider, AuthenticationInfo, ActiveUser>
    {
        public AuthenticationTokenController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
