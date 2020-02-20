using AspCore.Authentication.Abstract;
using AspCore.Authentication.Concrete;

namespace AspCoreTest.Authentication.Abstract
{
    public interface ICustomWebAuthenticationProvider : IInboundWebAuthenticationProvider<AuthenticationInfo>
    {
    }
}
