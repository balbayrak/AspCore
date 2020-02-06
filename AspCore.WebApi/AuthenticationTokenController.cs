using AspCore.Authentication.Abstract;
using AspCore.Authentication.Concrete;
using AspCore.Entities.User;
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
