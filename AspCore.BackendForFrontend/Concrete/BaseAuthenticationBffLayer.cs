using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Extension;
using System;
using System.Threading.Tasks;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseAuthenticationBffLayer<TAuthenticationInfo, TAuthenticationResult> : BaseBffLayer
        where TAuthenticationInfo : AuthenticationInfo
        where TAuthenticationResult : class, IAuthenticatedUser, new()
    {
        public abstract string authenticationRoute { get; }
        public BaseAuthenticationBffLayer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            apiControllerRoute = authenticationRoute;
        }

        public async Task<ServiceResult<TAuthenticationResult>> GetClientInfo(AuthenticationTicketInfo authenticationToken)
        {
            ServiceResult<TAuthenticationResult> result = new ServiceResult<TAuthenticationResult>();
            try
            {
                ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.GET_CLIENT_INFO;

                result = await ApiClient.PostRequest<ServiceResult<TAuthenticationResult>>(authenticationToken);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(FrontEndConstants.ERROR_MESSAGES.GET_USER_INFO_ERROR, ex);
            }
            return result;
        }

        public async Task<ServiceResult<AuthenticationTicketInfo>> AuthenticateClient(TAuthenticationInfo authenticationInfo)
        {
            ServiceResult<AuthenticationTicketInfo> result = new ServiceResult<AuthenticationTicketInfo>();
            try
            {
                ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.AUTHENTICATE_CLIENT;

                result = await ApiClient.PostRequest<ServiceResult<AuthenticationTicketInfo>>(authenticationInfo);

            }
            catch (Exception ex)
            {
                result.ErrorMessage(FrontEndConstants.ERROR_MESSAGES.AUTHENTICATE_CLIENT_ERROR, ex);
            }

            return result;
        }

    }
}
