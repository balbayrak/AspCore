using AspCore.Authentication.JWT.Abstract;
using AspCore.BackendForFrontend.Abstract;
using AspCore.BackendForFrontend.Concrete.Security.Claims;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Storage.Abstract;
using AspCore.Storage.Concrete;
using AspCore.Utilities;
using AspCore.Web.Authentication.Abstract;
using AspCore.Web.Configuration.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

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

        public StorageService StorageManager => LazyGetRequiredService(ref _storageService);
        private StorageService _storageService;

        protected TAuthenticatonBff AuthenticationBffLayer => LazyGetRequiredService(ref _authenticationBffLayer);
        private TAuthenticatonBff _authenticationBffLayer;

        protected IWebAuthenticationProvider<TAuthenticationInfo> AuthenticationProvider => LazyGetRequiredServiceByName(ref _authenticationProvider, AuthenticationProviderName);
        private IWebAuthenticationProvider<TAuthenticationInfo> _authenticationProvider;

        private readonly ITokenValidator<TAuthenticationResult> _tokenValidator;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IAuthenticationService _authenticationService;

        private readonly CookieConfigurationBuilder _cookieConfigurationBuilder;

        public BaseAuthenticationController(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            _tokenValidator = ServiceProvider.GetRequiredService<ITokenValidator<TAuthenticationResult>>();
            _httpContextAccessor = ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            _authenticationService = ServiceProvider.GetRequiredService<IAuthenticationService>();
            _cookieConfigurationBuilder = ServiceProvider.GetRequiredService<CookieConfigurationBuilder>();
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
                ServiceResult<AuthenticationTicketInfo> authenticationResult = AuthenticationBffLayer.AuthenticateClient((TAuthenticationInfo)serviceResult.Result).Result;

                if (authenticationResult.IsSucceededAndDataIncluded())
                {
                    ServiceResult<TAuthenticationResult> userResult = null;

                    if (_tokenValidator.ValidatePublicKey)
                        userResult = _tokenValidator.Validate(authenticationResult.Result, false);
                    else
                    {
                        userResult = AuthenticationBffLayer.GetClientInfo(authenticationResult.Result).Result;
                    }


                    ServiceResult<List<Claim>> apiClaimsResult = _tokenValidator.GetClaims(authenticationResult.Result);

                    if ((userResult != null && userResult.IsSucceeded && userResult.Result != null) && (apiClaimsResult.IsSucceeded && apiClaimsResult.Result != null))
                    {

                        var userIdClaim = apiClaimsResult.Result.FirstOrDefault(t => t.Type == AspCoreSecurityType.UserId);

                        if (userIdClaim == null)
                            throw new Exception("Api Claims must include 'AspCoreSecurityType.UserId' claim type");

                        var userNameClaim = apiClaimsResult.Result.FirstOrDefault(t => t.Type == AspCoreSecurityType.UserName);

                        if (userNameClaim == null)
                            throw new Exception("Api Claims must include 'AspCoreSecurityType.UserName' claim type");


                        var claims = new List<Claim>();

                        claims.Add(new Claim(AspCoreSecurityType.UserId, userIdClaim.Value));
                        claims.Add(new Claim(AspCoreSecurityType.UserName, userNameClaim.Value));
                        claims.Add(new Claim(AspCoreSecurityType.UserInfo, JsonConvert.SerializeObject(userResult.Result).CompressString()));

                        var claimsIdentity = new ClaimsIdentity(
               claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authticationProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            ExpiresUtc = DateTimeOffset.Now.AddMinutes(_cookieConfigurationBuilder.cookieOption.Expire),
                            IsPersistent = true,
                        };

                        var jwtJson = authenticationResult.Result.access_token.CompressString();


                        AuthenticationToken token = new AuthenticationToken();
                        token.Name = ApiConstants.Api_Keys.ACCESS_TOKEN;
                        token.Value = jwtJson;


                        var refreshJson = authenticationResult.Result.refresh_token.CompressString();


                        AuthenticationToken refreshToken = new AuthenticationToken();
                        refreshToken.Name = ApiConstants.Api_Keys.REFRESH_TOKEN;
                        refreshToken.Value = refreshJson;

                        var expiresAt = authenticationResult.Result.expires.ToString("o", CultureInfo.InvariantCulture);


                        AuthenticationToken expire = new AuthenticationToken();
                        expire.Name = ApiConstants.Api_Keys.EXPIRES;
                        expire.Value = expiresAt;


                        List<AuthenticationToken> tokens = new List<AuthenticationToken>();

                        tokens.Add(token);
                        tokens.Add(refreshToken);
                        tokens.Add(expire);


                        authticationProperties.StoreTokens(tokens);

                        var principal = new ClaimsPrincipal(claimsIdentity);

                        _authenticationService.SignInAsync(HttpContext,
                         CookieAuthenticationDefaults.AuthenticationScheme,
                         principal,
                         authticationProperties);


                        Response.Redirect(AuthenticationProvider.mainPageUrl);
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
            else
            {
                Response.Redirect(AuthenticationProvider.loginPageUrl);
            }
        }

        public void Login(TAuthenticationInfo authenticationInfo = null)
        {
            var token = _httpContextAccessor.HttpContext.GetTokenAsync(ApiConstants.Api_Keys.ACCESS_TOKEN).Result;

            if (string.IsNullOrEmpty(token))
            {
                Authenticate(authenticationInfo);
            }
            else
            {
                Response.Redirect(AuthenticationProvider.mainPageUrl);
            }
        }

        public virtual void LogOut()
        {
            _httpContextAccessor.HttpContext.SignOutAsync();

            if (StorageManager.CookieService != null)
                StorageManager.CookieService.RemoveAll();

            Response.Redirect("Login");
        }
    }
}
