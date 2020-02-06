using System;
using System.Threading.Tasks;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Authentication.Concrete;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.Constants;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Extension;

namespace AspCore.BackendForFrontend.Concrete
{
    public class UserBffLayer : BaseBffLayer, IUserBffLayer
    {
        public UserBffLayer()
        {
            apiControllerRoute = "api/AuthenticationToken";
        }

        public async Task<ServiceResult<ActiveUser>> GetClientInfo(AuthenticationTokenResponse authenticationToken)
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

        public async Task<ServiceResult<AuthenticationTokenResponse>> AuthenticateClient(AuthenticationInfo authenticationInfo)
        {
            ServiceResult<AuthenticationTokenResponse> result = new ServiceResult<AuthenticationTokenResponse>();
            try
            {
                apiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.AUTHENTICATE_CLIENT;

                result = await apiClient.PostRequest<ServiceResult<AuthenticationTokenResponse>>(authenticationInfo);

            }
            catch (Exception ex)
            {
                result.ErrorMessage(FrontEndConstants.ERROR_MESSAGES.AUTHENTICATE_CLIENT_ERROR, ex);
            }

            return result;
        }

    }
}
