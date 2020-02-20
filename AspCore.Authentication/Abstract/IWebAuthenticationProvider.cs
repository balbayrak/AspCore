using AspCore.Authentication.Concrete;

namespace AspCore.Authentication.Abstract
{
    public interface IWebAuthenticationProvider<TInput>
        where TInput : AuthenticationInfo
    {
        string loginPageUrl { get; }

        string apiAuthenticationType { get; }

        string mainPageUrl { get; }
    }
}
