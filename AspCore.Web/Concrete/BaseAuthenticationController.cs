using AspCore.Authentication.JWT.Abstract;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Caching.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Web.Authentication.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Web.Concrete
{
    public abstract class BaseAuthenticationController<TAuthenticationInfo, TAuthenticationResult, TAuthenticatonBff> : Controller
        where TAuthenticationInfo : AuthenticationInfo
        where TAuthenticationResult : class, IJWTEntity, new()
        where TAuthenticatonBff : IAuthenticationBffLayer<TAuthenticationInfo, TAuthenticationResult>
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

        protected TService LazyGetRequiredServiceByName<TService>(ref TService reference, string name)
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

        protected ICookieService CookieService => LazyGetRequiredService(ref _cookieService);
        private ICookieService _cookieService;

        protected TAuthenticatonBff AuthenticationBffLayer => LazyGetRequiredService(ref _authenticationBffLayer);
        private TAuthenticatonBff _authenticationBffLayer;

        protected IWebAuthenticationProvider<TAuthenticationInfo> AuthenticationProvider => LazyGetRequiredServiceByName(ref _authenticationProvider, AuthenticationProviderName);
        private IWebAuthenticationProvider<TAuthenticationInfo> _authenticationProvider;

        private ITokenValidator<TAuthenticationResult> _tokenValidator;

        public BaseAuthenticationController(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            _tokenValidator = ServiceProvider.GetRequiredService<ITokenValidator<TAuthenticationResult>>();
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

                CookieService.SetObjectAsync(ApiConstants.Api_Keys.APP_USER_STORAGE_KEY, tokenKey, DateTime.Now.AddDays(1), false);

                ServiceResult<AuthenticationToken> authenticationResult = AuthenticationBffLayer.AuthenticateClient((TAuthenticationInfo)serviceResult.Result).Result;

                if (authenticationResult.IsSucceededAndDataIncluded())
                {
                    AuthenticationBffLayer.SetAuthenticationToken(tokenKey, authenticationResult.Result);


                    ServiceResult<TAuthenticationResult> userResult = null;

                    if (_tokenValidator.ValidatePublicKey)
                        userResult = _tokenValidator.Validate(authenticationResult.Result, false);
                    else
                    {
                        userResult = AuthenticationBffLayer.GetClientInfo(authenticationResult.Result).Result;
                    }


                    if (userResult != null && userResult.IsSucceeded && userResult.Result != null)
                    {
                        string activeUserUId = FrontEndConstants.STORAGE_CONSTANT.APPLICATION_USER + "_" + tokenKey;

                        CacheService.SetObjectAsync(activeUserUId, userResult.Result, DateTime.Now.AddDays(1), false);

                        Response.Redirect(AuthenticationProvider.mainPageUrl);
                    }
                    else
                    {
                        CacheService.RemoveAllAsync();
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
            string tokenKey = CookieService.GetObject<string>(ApiConstants.Api_Keys.APP_USER_STORAGE_KEY);

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
                CookieService.RemoveAllAsync();
                CacheService.RemoveAllAsync();
                 
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
