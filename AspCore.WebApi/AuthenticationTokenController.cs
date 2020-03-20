using AspCore.Entities.Authentication;
using AspCore.Entities.User;
using AspCore.WebApi.Authentication.Providers.Abstract;
using AspCore.WebApi.Security.Abstract;

namespace AspCore.WebApi
{
    public class AuthenticationTokenController : BaseJWTAuthenticationController<IActiveUserAuthenticationProvider,IActiveUserTokenGenerator, AuthenticationInfo, ActiveUser>
    {
        public AuthenticationTokenController()
        {

        }
    }
}
