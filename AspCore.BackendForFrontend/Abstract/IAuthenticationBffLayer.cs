using AspCore.Entities.Authentication;
using AspCore.Entities.General;
using System.Threading.Tasks;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IAuthenticationBffLayer<TAuthenticationInfo, TAuthenticationResult> : IBffLayer
        where TAuthenticationInfo : AuthenticationInfo
    {
        Task<ServiceResult<AuthenticationToken>> AuthenticateClient(TAuthenticationInfo authenticationInfo);
        Task<ServiceResult<TAuthenticationResult>> GetClientInfo(AuthenticationToken authenticationToken);

    }
}
