using AspCore.Entities.Authentication;
using AspCore.Entities.General;

namespace AspCore.Web.Authentication.Abstract
{
    public interface IOutboundWebAuthenticationProvider<TInput> : IWebAuthenticationProvider<TInput>
         where TInput : AuthenticationInfo
    {
        ServiceResult<TInput> GetAuthenticationFormInfo();
    }
}
