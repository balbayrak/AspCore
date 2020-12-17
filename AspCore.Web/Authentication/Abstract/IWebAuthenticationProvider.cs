using AspCore.Entities.Authentication;

namespace AspCore.Web.Authentication.Abstract
{
    public interface IWebAuthenticationProvider<TInput>
        where TInput : AuthenticationInfo
    {
        string loginPageUrl { get; }
        string firstPageUrl { get; }

        string apiAuthenticationType { get; }

        string mainPageUrl { get; }
    }
}
