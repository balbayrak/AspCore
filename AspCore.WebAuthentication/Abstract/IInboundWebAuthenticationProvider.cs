using AspCore.Entities.Authentication;
using AspCore.Entities.General;

namespace AspCore.WebAuthentication.Abstract
{
    public interface IInboundWebAuthenticationProvider<TInput> : IWebAuthenticationProvider<TInput>
         where TInput : AuthenticationInfo
    {
        ServiceResult<TInput> GetAuthenticationFormInfo(TInput input);
    }
}
