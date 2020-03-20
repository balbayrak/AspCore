using AspCore.Entities.Authentication;
using AspCore.Web.Authentication.Abstract;

namespace AspCoreTest.Authentication.Abstract
{
    public interface ICustomWebAuthenticationProvider : IInboundWebAuthenticationProvider<AuthenticationInfo>
    {
    }
}
