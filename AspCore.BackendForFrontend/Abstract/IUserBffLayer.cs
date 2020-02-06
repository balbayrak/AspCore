using System.Threading.Tasks;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Authentication.Concrete;
using AspCore.Entities.General;
using AspCore.Entities.User;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IUserBffLayer : IBFFLayer
    {
        Task<ServiceResult<AuthenticationTokenResponse>> AuthenticateClient(AuthenticationInfo authenticationInfo);
        Task<ServiceResult<ActiveUser>> GetClientInfo(AuthenticationTokenResponse authenticationToken);

    }
}
