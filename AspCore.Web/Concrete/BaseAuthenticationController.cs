using AspCore.BackendForFrontend.Abstract;
using AspCore.Caching.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Web.Authentication.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Web.Concrete
{
    public abstract class BaseAuthenticationController<TAuthenticationInfo> : Controller
         where TAuthenticationInfo : AuthenticationInfo
    {
        public IServiceProvider ServiceProvider { get; set; }
        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
            => LazyGetRequiredService(typeof(TService), ref reference);

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        protected TService LazyGetRequiredServiceByName<TService>(ref TService reference,string name)
           => LazyGetRequiredServiceByName(typeof(TService), ref reference, name);

        protected TRef LazyGetRequiredServiceByName<TRef>(Type serviceType, ref TRef reference, string name)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetServiceByName<TRef>(name);
                    }
                }
            }

            return reference;
        }

        public abstract string AuthenticationProviderName { get; }

        protected ICacheService CacheService => LazyGetRequiredService(ref _cacheService);
        private ICacheService _cacheService;

        protected IUserBffLayer UserBffLayer => LazyGetRequiredService(ref _userBffLayer);
        private IUserBffLayer _userBffLayer;

        protected IWebAuthenticationProvider<TAuthenticationInfo> AuthenticationProvider => LazyGetRequiredServiceByName(ref _authenticationProvider,AuthenticationProviderName);
        private IWebAuthenticationProvider<TAuthenticationInfo> _authenticationProvider;

        public BaseAuthenticationController(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        private void Authenticate(TAuthenticationInfo authenticationInfo = null)
        {
            ServiceResult<AuthenticationInfo> serviceResult = null;

            if (AuthenticationProvider is IInboundWebAuthenticationProvider<AuthenticationInfo>)
            {
                serviceResult = ((IInboundWebAuthenticationProvider<AuthenticationInfo>)AuthenticationProvider).GetAuthenticationFormInfo(authenticationInfo);
            }
            else
            {
                serviceResult = ((IOutboundWebAuthenticationProvider<AuthenticationInfo>)AuthenticationProvider).GetAuthenticationFormInfo();
            }

            if (serviceResult.IsSucceeded)
            {
                string tokenKey = Guid.NewGuid().ToString("N");

                CacheService.SetObject(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY, tokenKey, DateTime.Now.AddHours(1), false);

                ServiceResult<AuthenticationToken> authenticationResult = UserBffLayer.AuthenticateClient(serviceResult.Result).Result;
                if (authenticationResult.IsSucceededAndDataIncluded())
                {
                    UserBffLayer.SetAuthenticationToken(tokenKey, authenticationResult.Result);

                    ServiceResult<ActiveUser> userResult = UserBffLayer.GetClientInfo(authenticationResult.Result).Result;

                    if (userResult != null && userResult.IsSucceeded && userResult.Result != null)
                    {
                        string activeUserUId = FrontEndConstants.STORAGE_CONSTANT.COOKIE_USER + "_" + tokenKey;

                        CacheService.SetObject(activeUserUId, userResult.Result, DateTime.Now.AddHours(1), false);

                        Response.Redirect(AuthenticationProvider.mainPageUrl);
                    }
                    else
                    {
                        CacheService.RemoveAll();
                        Response.Redirect(AuthenticationProvider.loginPageUrl);
                    }
                }
                else
                {
                    Response.Redirect(AuthenticationProvider.loginPageUrl);
                }
            }
            else
            {
                Response.Redirect(AuthenticationProvider.loginPageUrl);
            }
        }
        public void Login(TAuthenticationInfo authenticationInfo = null)
        {
            string tokenKey = CacheService.GetObject<string>(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY);

            if (string.IsNullOrEmpty(tokenKey) || (!string.IsNullOrEmpty(tokenKey) && CacheService.GetObject<AuthenticationToken>(tokenKey) == null))
            {
                ClearStorage();
                Authenticate(authenticationInfo);
            }
            else
            {
                Response.Redirect(AuthenticationProvider.mainPageUrl);
            }
        }
        private void ClearStorage()
        {
            try
            {
                CacheService.RemoveAll();
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
