using AspCore.ApiClient.Entities.Concrete;
using AspCore.Authentication.Abstract;
using AspCore.Authentication.Concrete;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Storage.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AspCore.Web.Concrete
{
    public abstract class BaseAuthenticationController<TAuthenticationInfo> : Controller
         where TAuthenticationInfo : AuthenticationInfo
    {
        private readonly IUserBffLayer _userBffLayer;
        protected readonly IStorage Storage;
        public abstract string AuthenticationProviderName { get; }
        private readonly IWebAuthenticationProvider<TAuthenticationInfo> _authenticationProvider;

        public BaseAuthenticationController()
        {
            _userBffLayer = DependencyResolver.Current.GetService<IUserBffLayer>();
            Storage = DependencyResolver.Current.GetService<IStorage>();
            _authenticationProvider = DependencyResolver.Current.GetServiceByName<IWebAuthenticationProvider<TAuthenticationInfo>>(AuthenticationProviderName);

        }
        private void Authenticate(TAuthenticationInfo authenticationInfo = null)
        {
            ServiceResult<AuthenticationInfo> serviceResult = null;

            if (authenticationInfo != null && !string.IsNullOrEmpty(authenticationInfo.UserName) && !string.IsNullOrEmpty(authenticationInfo.Password))
            {
                serviceResult = ((IInboundWebAuthenticationProvider<AuthenticationInfo>)_authenticationProvider).GetAuthenticationFormInfo(authenticationInfo);
            }
            else
            {
                serviceResult = ((IOutboundWebAuthenticationProvider<AuthenticationInfo>)_authenticationProvider).GetAuthenticationFormInfo();
            }


            if (serviceResult.IsSucceeded)
            {
                string tokenKey = Guid.NewGuid().ToString("N");

                Storage.SetObject(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY, tokenKey, DateTime.Now.AddHours(1), false);

                ServiceResult<AuthenticationTokenResponse> authenticationResult = _userBffLayer.AuthenticateClient(serviceResult.Result).Result;
                if (authenticationResult.IsSucceededAndDataIncluded())
                {
                    _userBffLayer.SetAuthenticationToken(tokenKey, authenticationResult.Result);

                    ServiceResult<ActiveUser> userResult = _userBffLayer.GetClientInfo(authenticationResult.Result).Result;

                    if (userResult != null && userResult.IsSucceeded && userResult.Result != null)
                    {
                        string activeUserUId = FrontEndConstants.STORAGE_CONSTANT.COOKIE_USER + "_" + tokenKey;

                        Storage.SetObject(activeUserUId, userResult.Result, DateTime.Now.AddHours(1), false);

                        Response.Redirect(_authenticationProvider.mainPageUrl);
                    }
                }
                else
                {
                    Response.Redirect(_authenticationProvider.loginPageUrl);
                }
            }
            else
            {
                Response.Redirect(_authenticationProvider.loginPageUrl);
            }
        }
        public void Login(TAuthenticationInfo authenticationInfo = null)
        {
            string tokenKey = Storage.GetObject<string>(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY);

            if (string.IsNullOrEmpty(tokenKey) || (!string.IsNullOrEmpty(tokenKey) && Storage.GetObject<AuthenticationTokenResponse>(tokenKey) == null))
            {
                ClearStorage();
                Authenticate(authenticationInfo);
            }
            else
            {
                Response.Redirect(_authenticationProvider.mainPageUrl);
            }
        }
        private void ClearStorage()
        {
            try
            {
                Storage.RemoveAll();
            }
            catch
            {
                // ignored
            }
        }
        public virtual void LogOut()
        {
            ClearStorage();
            Response.Redirect("Login");
        }
    }
}
