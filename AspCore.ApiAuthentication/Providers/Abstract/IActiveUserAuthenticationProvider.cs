using AspCore.Entities.Authentication;
using AspCore.Entities.User;

namespace AspCore.ApiAuthentication.Providers.Abstract
{
    public interface IActiveUserAuthenticationProvider : IApiAuthenticationProvider<AuthenticationInfo, ActiveUser>
    {

    }
}
