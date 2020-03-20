using AspCore.ApiClient.Entities.Concrete;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Extension;
using System;
using System.Threading.Tasks;

namespace AspCore.BackendForFrontend.Concrete
{
    public class UserBffLayer : BaseBffLayer, IUserBffLayer
    {
        public UserBffLayer()
        {
            apiControllerRoute = "api/AuthenticationToken";
        }

        public async Task<ServiceResult<ActiveUser>> GetClientInfo(AuthenticationToken authenticationToken)
        {
            ServiceResult<ActiveUser> result = new ServiceResult<ActiveUser>();
            try
            {
                apiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.GET_CLIENT_INFO;

                result = await apiClient.PostRequest<ServiceResult<ActiveUser>>(authenticationToken, null, authenticationToken);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(FrontEndConstants.ERROR_MESSAGES.GET_USER_INFO_ERROR, ex);
            }
            return result;
        }

        public async Task<ServiceResult<AuthenticationToken>> AuthenticateClient(AuthenticationInfo authenticationInfo)
        {
            ServiceResult<AuthenticationToken> result = new ServiceResult<AuthenticationToken>();
            try
            {
                apiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.AUTHENTICATE_CLIENT;

                result = await apiClient.PostRequest<ServiceResult<AuthenticationToken>>(authenticationInfo);

            }
            catch (Exception ex)
            {
                result.ErrorMessage(FrontEndConstants.ERROR_MESSAGES.AUTHENTICATE_CLIENT_ERROR, ex);
            }

            return result;
        }

    }
}
