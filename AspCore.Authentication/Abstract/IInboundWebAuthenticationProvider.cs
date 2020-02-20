using AspCore.Authentication.Concrete;
using AspCore.Entities.General;

namespace AspCore.Authentication.Abstract
{
    public interface IInboundWebAuthenticationProvider<TInput> : IWebAuthenticationProvider<TInput>
         where TInput : AuthenticationInfo
    {
        ServiceResult<TInput> GetAuthenticationFormInfo(TInput input);
    }
}
