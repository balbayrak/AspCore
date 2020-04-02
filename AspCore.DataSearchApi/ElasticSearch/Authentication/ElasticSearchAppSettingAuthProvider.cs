using AspCore.DataSearchApi.General;
using AspCore.Dependency.Concrete;
using AspCore.ElasticSearch.Configuration;
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

namespace AspCore.DataSearchApi.ElasticSearch.Authentication
{
    public class ElasticSearchAppSettingAuthProvider : AppSettingsAuthenticationProvider<AuthenticationInfo, ElasticSearchApiJWTInfo, ElasticSearchApiOption>, IAppSettingsApiAuthenticationProvider<AuthenticationInfo, ElasticSearchApiJWTInfo, ElasticSearchApiOption>
    {
        private IHttpContextAccessor _httpContextAccessor;
        private ITokenGenerator<ElasticSearchApiJWTInfo> _tokenGenerator;
        public ElasticSearchAppSettingAuthProvider(string configurationKey, ElasticSearchApiOption option = null) : base(configurationKey, option)
        {
            _httpContextAccessor = DependencyResolver.Current.GetService<IHttpContextAccessor>();
            _tokenGenerator = DependencyResolver.Current.GetService<ITokenGenerator<ElasticSearchApiJWTInfo>>();
        }

        public override ServiceResult<ElasticSearchApiJWTInfo> AuthenticateClient(AuthenticationInfo input)
        {
            ServiceResult<ElasticSearchApiJWTInfo> serviceResult = new ServiceResult<ElasticSearchApiJWTInfo>();
            try
            {
                if (_option != null)
                {
                    AuthorizedClient authorizedClient = _option.AuthorizedClients.FirstOrDefault(t => t.ClientAuthenticationInfo.Username == input.UserName
                    && t.ClientAuthenticationInfo.Password == input.Password);

                    ElasticSearchApiJWTInfo elasticSearchApiJWTInfo = null;
                    //user authorize all cache nodes
                    if (authorizedClient != null)
                    {
                        elasticSearchApiJWTInfo = new ElasticSearchApiJWTInfo();
                        elasticSearchApiJWTInfo.isAuthorizedAllActions = true;
                    }
                    else
                    {
                        List<AuthorizedElasticSearchIndex> authorizedElasticSearchIndices = _option.ElasticSearchIndices.Where(t => (t.AuthorizedClients != null && t.AuthorizedClients.Any(ac => ac.ClientAuthenticationInfo != null && ac.ClientAuthenticationInfo.Username.Equals(input.UserName, StringComparison.InvariantCultureIgnoreCase)
                                      && ac.ClientAuthenticationInfo.Password.Equals(input.Password, StringComparison.InvariantCultureIgnoreCase))))
                            .Select(t => new AuthorizedElasticSearchIndex
                            {
                                indexKey = t.IndexKey,
                                actions = t.AuthorizedClients.FirstOrDefault(ac => ac.ClientAuthenticationInfo != null && ac.ClientAuthenticationInfo.Username.Equals(input.UserName, StringComparison.InvariantCultureIgnoreCase)
                                && ac.ClientAuthenticationInfo.Password.Equals(input.Password, StringComparison.InvariantCultureIgnoreCase))?.AuthorizedActions
                            })
                            .ToList();
                        if (authorizedElasticSearchIndices != null && authorizedElasticSearchIndices.Count > 0)
                        {
                            elasticSearchApiJWTInfo = new ElasticSearchApiJWTInfo();
                            elasticSearchApiJWTInfo.isAuthorizedAllActions = false;
                            elasticSearchApiJWTInfo.AuthorizedElasticSearchIndices = authorizedElasticSearchIndices;
                        }
                    }

                    if (elasticSearchApiJWTInfo != null)
                    {
                        serviceResult.IsSucceeded = true;
                        serviceResult.Result = elasticSearchApiJWTInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(DataSearchApiConstants.ErrorMessages.AUTHENTICATE_EXCEPTION, ex);
            }

            return serviceResult;
        }

        public override ServiceResult<bool> AuthorizeActionInternal(string actionName, IDictionary<string, object> arguments = null)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            result.IsSucceeded = true;
            result.Result = true;

            if (actionName.Equals(ApiConstants.DataSearchApi_Urls.CREATE_ACTION_NAME, StringComparison.InvariantCultureIgnoreCase)
                || actionName.Equals(ApiConstants.DataSearchApi_Urls.DELETE_ACTION_NAME, StringComparison.InvariantCultureIgnoreCase)
                || actionName.Equals(ApiConstants.DataSearchApi_Urls.READ_ACTION_NAME, StringComparison.InvariantCultureIgnoreCase)
                || actionName.Equals(ApiConstants.DataSearchApi_Urls.GETDATA_ACTION_NAME, StringComparison.InvariantCultureIgnoreCase)
                || actionName.Equals(ApiConstants.DataSearchApi_Urls.INIT_INDEX_ACTION_NAME, StringComparison.InvariantCultureIgnoreCase)
                || actionName.Equals(ApiConstants.DataSearchApi_Urls.UPDATE_ACTION_NAME, StringComparison.InvariantCultureIgnoreCase))
            {
                result.IsSucceeded = false;
                result.Result = false;


                string jwt = _httpContextAccessor.HttpContext.GetJWTToken();
                if (!string.IsNullOrEmpty(jwt))
                {
                    ServiceResult<ElasticSearchApiJWTInfo> tokenResult = _tokenGenerator.GetJWTInfo(new AuthenticationToken
                    {
                        access_token = jwt
                    });
                    if (tokenResult.IsSucceededAndDataIncluded())
                    {
                        bool authorized = (tokenResult.Result.isAuthorizedAllActions.HasValue && tokenResult.Result.isAuthorizedAllActions.Value) ||
                            tokenResult.Result.AuthorizedElasticSearchIndices.FirstOrDefault(t => t.actions.Any(tt => tt.Equals(actionName, StringComparison.InvariantCultureIgnoreCase))) != null;

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
