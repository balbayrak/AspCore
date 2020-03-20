using AspCore.Entities.Authentication;
using AspCore.WebAuthentication.Abstract;

namespace AspCoreTest.Authentication.Abstract
{
    public interface ICustomWebAuthenticationProvider : IInboundWebAuthenticationProvider<AuthenticationInfo>
    {
    }
}
