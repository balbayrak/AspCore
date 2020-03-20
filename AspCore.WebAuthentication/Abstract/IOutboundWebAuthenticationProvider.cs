using AspCore.Entities.Authentication;
using AspCore.Entities.General;

namespace AspCore.WebAuthentication.Abstract
{
    public interface IOutboundWebAuthenticationProvider<TInput> : IWebAuthenticationProvider<TInput>
         where TInput : AuthenticationInfo
    {
        ServiceResult<TInput> GetAuthenticationFormInfo();
    }
}
