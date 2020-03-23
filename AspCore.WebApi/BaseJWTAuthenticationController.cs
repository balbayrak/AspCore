using AspCore.ApiClient.Entities.Concrete;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.WebApi.Authentication.Abstract;
using AspCore.WebApi.Authentication.JWT.Concrete;
using AspCore.WebApi.Authentication.Providers.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AspCore.WebApi
{
    public abstract class BaseJWTAuthenticationController<TAuthenticationProvider, TTokenGenerator, TInput, TOutput> : BaseController
          where TInput : AuthenticationInfo
          where TOutput : class, IJWTEntity, new()
          where TTokenGenerator : ITokenGenerator<TOutput>
          where TAuthenticationProvider : IApiAuthenticationProvider<TInput, TOutput>
    {
        public virtual string authenticationProviderName { get; }
        protected TAuthenticationProvider authenticationProvider { get; private set; }

        private TTokenGenerator _tokenGenerator;
        public BaseJWTAuthenticationController()
        {
            _tokenGenerator = DependencyResolver.Current.GetService<TTokenGenerator>(); ;
            authenticationProvider = DependencyResolver.Current.GetService<TAuthenticationProvider>();
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName(ApiConstants.Urls.AUTHENTICATE_CLIENT)]
        [ProducesResponseType(typeof(ServiceResult<AuthenticationToken>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult AuthenticateClient([FromBody]TInput authenticationInput)
        {

            ServiceResult<AuthenticationToken> serviceResult = new ServiceResult<AuthenticationToken>();

            if (authenticationProvider == null)
            {
                string authProviderName = authenticationInput.authenticationProvider;
                if (string.IsNullOrEmpty(authProviderName))
                {
                    authProviderName = authenticationProviderName;
                }
                authenticationProvider = DependencyResolver.Current.GetServiceByName<TAuthenticationProvider>(authProviderName);
            }

            if (authenticationProvider != null)
            {
                try
                {
                    ServiceResult<TOutput> userInfoResult = authenticationProvider.Authenticate(authenticationInput);

                    if (userInfoResult.IsSucceededAndDataIncluded())
                    {
                        ServiceResult<AuthenticationToken> authenticationTokenResult = _tokenGenerator.CreateToken(userInfoResult.Result);

                        if (authenticationTokenResult.IsSucceededAndDataIncluded())
                        {
                            serviceResult.IsSucceeded = true;
                            serviceResult.Result = new AuthenticationToken();
                            serviceResult.Result.access_token = authenticationTokenResult.Result.access_token;
                            serviceResult.Result.refresh_token = authenticationTokenResult.Result.refresh_token;
                            serviceResult.Result.expires = authenticationTokenResult.Result.expires;
                        }
                        else
                        {
                            serviceResult.ErrorMessage = authenticationTokenResult.ErrorMessage;
                            serviceResult.ExceptionMessage = authenticationTokenResult.ExceptionMessage;
                        }
                    }
                    else
                    {
                        serviceResult.ErrorMessage = userInfoResult.ErrorMessage;
                        serviceResult.ExceptionMessage = userInfoResult.ExceptionMessage;
                    }
                }
                catch (Exception ex)
                {
                    serviceResult.ErrorMessage(SecurityConstants.TOKEN_SETTING_OPTIONS.TOKEN_CREATE_EXCEPTION, ex);
                }
            }
            else
            {
                serviceResult.ErrorMessage = SecurityConstants.TOKEN_SETTING_OPTIONS.AUTHENTICATION_PROVIDER_NOT_FOUND;
            }


            return serviceResult.ToHttpResponse();
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName(ApiConstants.Urls.REFRESH_TOKEN)]
        [ProducesResponseType(typeof(ServiceResult<AuthenticationToken>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult RefreshToken([FromBody]AuthenticationToken authenticationToken)
        {
            ServiceResult<AuthenticationToken> serviceResult = new ServiceResult<AuthenticationToken>();
            try
            {
                ServiceResult<AuthenticationToken> newAuthenticationTokenResult = _tokenGenerator.RefreshToken(new AuthenticationToken
                {
                    access_token = authenticationToken.access_token,
                    expires = authenticationToken.expires,
                    refresh_token = authenticationToken.refresh_token
                });

                if (newAuthenticationTokenResult.IsSucceededAndDataIncluded())
                {
                    serviceResult.IsSucceeded = true;
                    serviceResult.Result = new AuthenticationToken();
                    serviceResult.Result.access_token = newAuthenticationTokenResult.Result.access_token;
                    serviceResult.Result.refresh_token = newAuthenticationTokenResult.Result.refresh_token;
                    serviceResult.Result.expires = newAuthenticationTokenResult.Result.expires;
                }
                else
                {
                    serviceResult.ErrorMessage = newAuthenticationTokenResult.ErrorMessage;
                    serviceResult.ExceptionMessage = newAuthenticationTokenResult.ExceptionMessage;
                }
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(SecurityConstants.TOKEN_SETTING_OPTIONS.REFRESH_TOKEN__CREATE_EXCEPTION, ex);
            }
            return serviceResult.ToHttpResponse();
        }


        [HttpPost]
        [ActionName(ApiConstants.Urls.GET_CLIENT_INFO)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize]
        public IActionResult GetClientInfo([FromBody]AuthenticationToken authenticationToken)
        {
            ServiceResult<TOutput> serviceResult = new ServiceResult<TOutput>();

            if (authenticationToken != null && !string.IsNullOrEmpty(authenticationToken.access_token))
            {
                try
                {
                    serviceResult = _tokenGenerator.GetJWTInfo(new AuthenticationToken
                    {
                        access_token = authenticationToken.access_token,
                        expires = authenticationToken.expires,
                        refresh_token = authenticationToken.refresh_token
                    });
                }
                catch (Exception ex)
                {
                    serviceResult.ErrorMessage(SecurityConstants.TOKEN_SETTING_OPTIONS.REFRESH_TOKEN__CREATE_EXCEPTION, ex);
                }
            }
            else
            {
                serviceResult.IsSucceeded = true;
                serviceResult.Result = null;
            }
            return serviceResult.ToHttpResponse();
        }
    }
}
