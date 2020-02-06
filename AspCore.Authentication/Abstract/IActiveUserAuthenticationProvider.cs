using AspCore.Authentication.Concrete;
using AspCore.Entities.User;

namespace AspCore.Authentication.Abstract
{
    public interface IActiveUserAuthenticationProvider : IApiAuthenticationProvider<AuthenticationInfo, ActiveUser>
    {

    }
}
