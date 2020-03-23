using AspCore.Entities.Authentication;
using AspCore.Entities.User;

namespace AspCore.WebApi.Authentication.Providers.Abstract
{
    public interface IActiveUserAuthenticationProvider : IApiAuthenticationProvider<AuthenticationInfo, ActiveUser>
    {

    }
}
