using AspCore.Authentication.Concrete;
using AspCore.Entities.General;

namespace AspCore.Authentication.Abstract
{
    public interface IWebAuthenticationProvider<TInput>
        where TInput : AuthenticationInfo
    {
        ServiceResult<TInput> GetAuthenticationFormInfo();

        string loginPageUrl { get; }

        string apiAuthenticationType { get; }

        string mainPageUrl { get; }

    }
}
