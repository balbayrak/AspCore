using AspCore.Entities.Authentication;

namespace AspCore.WebAuthentication.Abstract
{
    public interface IWebAuthenticationProvider<TInput>
        where TInput : AuthenticationInfo
    {
        string loginPageUrl { get; }

        string apiAuthenticationType { get; }

        string mainPageUrl { get; }
    }
}
