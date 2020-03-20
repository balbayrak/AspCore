using AspCore.ApiClient.Entities.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.General;
using AspCore.Entities.User;
using System.Threading.Tasks;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IUserBffLayer : IBFFLayer
    {
        Task<ServiceResult<AuthenticationToken>> AuthenticateClient(AuthenticationInfo authenticationInfo);
        Task<ServiceResult<ActiveUser>> GetClientInfo(AuthenticationToken authenticationToken);

    }
}
