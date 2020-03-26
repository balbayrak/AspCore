using AspCore.CacheApi.General;
using AspCore.CacheEntityAccess.Configuration;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.WebApi.Authentication.Abstract;
using AspCore.WebApi.Authentication.Providers.Abstract;
using AspCore.WebApi.Authentication.Providers.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.CacheEntityApi.Authentication
{
    public class CacheApiAppSettingAuthProvider : AppSettingsAuthenticationProvider<AuthenticationInfo, CacheApiJWTInfo, CacheApiOption>, IAppSettingsApiAuthenticationProvider<AuthenticationInfo, CacheApiJWTInfo, CacheApiOption>
    {
        private IHttpContextAccessor _httpContextAccessor;
        private ITokenGenerator<CacheApiJWTInfo> _tokenGenerator;
        public CacheApiAppSettingAuthProvider(string configurationKey, CacheApiOption option = null) : base(configurationKey, option)
        {
            _httpContextAccessor = DependencyResolver.Current.GetService<IHttpContextAccessor>();
            _tokenGenerator = DependencyResolver.Current.GetService<ITokenGenerator<CacheApiJWTInfo>>();
        }

        public override ServiceResult<CacheApiJWTInfo> AuthenticateClient(AuthenticationInfo input)
        {
            ServiceResult<CacheApiJWTInfo> serviceResult = new ServiceResult<CacheApiJWTInfo>();
            try
            {
                if (_option != null)
                {
                    AuthorizedClient authorizedClient = _option.AuthorizedClients.FirstOrDefault(t => t.ClientAuthenticationInfo.Username == input.UserName
                    && t.ClientAuthenticationInfo.Password == input.Password);

                    CacheApiJWTInfo cacheApiJWTInfo = null;
                    //user authorize all cache nodes
                    if (authorizedClient != null)
                    {
                        cacheApiJWTInfo = new CacheApiJWTInfo();
                        cacheApiJWTInfo.isAuthorizedAllActions = true;
                    }
                    else
                    {
                        List<AuthorizedCacheNodes> authorizedCacheNodes = _option.CacheNodes.Where(t => (t.AuthorizedClients != null && t.AuthorizedClients.Any(ac => ac.ClientAuthenticationInfo != null && ac.ClientAuthenticationInfo.Username.Equals(input.UserName, StringComparison.InvariantCultureIgnoreCase)
                                      && ac.ClientAuthenticationInfo.Password.Equals(input.Password, StringComparison.InvariantCultureIgnoreCase))))
                            .Select(t => new AuthorizedCacheNodes
                            {
                                cacheKey = t.Cachekey,
                                actions = t.AuthorizedClients.FirstOrDefault(ac => ac.ClientAuthenticationInfo != null && ac.ClientAuthenticationInfo.Username.Equals(input.UserName, StringComparison.InvariantCultureIgnoreCase)
                                && ac.ClientAuthenticationInfo.Password.Equals(input.Password, StringComparison.InvariantCultureIgnoreCase))?.AuthorizedActions
                            })
                            .ToList();
                        if (authorizedCacheNodes != null && authorizedCacheNodes.Count > 0)
                        {
                            cacheApiJWTInfo = new CacheApiJWTInfo();
                            cacheApiJWTInfo.isAuthorizedAllActions = false;
                            cacheApiJWTInfo.AuthorizedCacheNodes = authorizedCacheNodes;
                        }
                    }

                    if (cacheApiJWTInfo != null)
                    {
                        serviceResult.IsSucceeded = true;
                        serviceResult.Result = cacheApiJWTInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(CacheApiConstants.ErrorMessages.AUTHENTICATE_EXCEPTION, ex);
            }

            return serviceResult;
        }

        public override ServiceResult<bool> AuthorizeActionInternal(string actionName, IDictionary<string, object> arguments = null)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            result.IsSucceeded = true;
            result.Result = true;

            if (actionName.Equals(ApiConstants.CacheApi_Urls.CREATE_ACTION_NAME, StringComparison.InvariantCultureIgnoreCase)
                || actionName.Equals(ApiConstants.CacheApi_Urls.DELETE_ACTION_NAME, StringComparison.InvariantCultureIgnoreCase)
                || actionName.Equals(ApiConstants.CacheApi_Urls.READ_ACTION_NAME, StringComparison.InvariantCultureIgnoreCase)
                || actionName.Equals(ApiConstants.CacheApi_Urls.GETDATA_ACTION_NAME, StringComparison.InvariantCultureIgnoreCase)
                || actionName.Equals(ApiConstants.CacheApi_Urls.MIN_MAX_ACTION_NAME, StringComparison.InvariantCultureIgnoreCase)
                || actionName.Equals(ApiConstants.CacheApi_Urls.UPDATE_ACTION_NAME, StringComparison.InvariantCultureIgnoreCase))
            {
                result.IsSucceeded = false;
                result.Result = false;


                string jwt = _httpContextAccessor.HttpContext.GetJWTToken();
                if (!string.IsNullOrEmpty(jwt))
                {
                    ServiceResult<CacheApiJWTInfo> tokenResult = _tokenGenerator.GetJWTInfo(new AuthenticationToken
                    {
                        access_token = jwt
                    });
                    if (tokenResult.IsSucceededAndDataIncluded())
                    {
                        bool authorized = (tokenResult.Result.isAuthorizedAllActions.HasValue && tokenResult.Result.isAuthorizedAllActions.Value) ||
                            tokenResult.Result.AuthorizedCacheNodes.FirstOrDefault(t => t.actions.Any(tt => tt.Equals(actionName, StringComparison.InvariantCultureIgnoreCase))) != null;

                        result.Result = authorized;
                        result.IsSucceeded = authorized;

                    }
                    else
                    {
                        result.ErrorMessage = tokenResult.ErrorMessage;
                        result.ExceptionMessage = tokenResult.ExceptionMessage;
                    }
                }
            }

            return result;
        }
    }
}
