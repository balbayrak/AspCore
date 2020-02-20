using AspCore.Authentication.Concrete;
using AspCore.Entities.General;

namespace AspCore.Authentication.Abstract
{
    public interface IOutboundWebAuthenticationProvider<TInput> : IWebAuthenticationProvider<TInput>
         where TInput : AuthenticationInfo
    {
        ServiceResult<TInput> GetAuthenticationFormInfo();
    }
}
