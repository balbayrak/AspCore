using System;
using System.Net.Http;
using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities.Abstract;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Caching.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using Microsoft.AspNetCore.Http;

namespace AspCore.ApiClient
{
    public abstract class AuthenticatedApiClient<TOutput, TOption> : ApiClient<TOption>, IAuthenticatedApiClient
         where TOption : class, IApiClientConfiguration, new()
    {
        private string apiConstantKey;

        /// <summary>
        /// This value used for token stores injected storage how many minutes. 
        /// If this value is null, expire time calculate api token response expire time plus 10 minutes.
        /// this value must be bigger than api token response expire time.
        /// Default value is 30 minutes all storage except for MemoryCache. Memory cache storage is 1 day.
        /// </summary>
        public DateTime? tokenStrorageExpireTime { get; set; }

        /// <summary>
        /// Token stores injected storage with this key.
        /// Default value : "ApiAccessToken" + "_" + apikey
        /// </summary>
        public string tokenStorageKey { get; set; }

        public abstract string AuthenticationBaseUrl { get; set; }
        public abstract string AuthenticationController { get; set; }
        public abstract string AuthenticationRefreshController { get; set; }


        public AuthenticatedApiClient(IHttpContextAccessor httpContextAccessor, IConfigurationAccessor configurationAccessor, ICacheService cacheService, string apiKey) : base(httpContextAccessor, configurationAccessor,cacheService,apiKey)
        {
            InitializeAuthenticatedClient(apiKey);
        }

        public abstract AuthenticationToken GetTokenResponse(TOutput outputObj);

        public abstract AuthenticationToken AuthenticateClient(AuthenticationInfo input, Func<TOutput, AuthenticationToken> func, bool forceAuthentication, bool refreshToken);

        public override AuthenticationToken Authenticate(HttpClient client, bool forceAuthentication, bool refreshToken)
        {
            AuthenticationToken tokenResponse = null;


            if (!forceAuthentication)
            {
                tokenResponse = AccessTokenService.GetObject<AuthenticationToken>(tokenStorageKey);
            }
            else
            {
                tokenResponse = AuthenticateClient(authenticationInfo, GetTokenResponse, forceAuthentication, refreshToken);

                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.access_token))
                {
                    if (tokenStrorageExpireTime == null || (tokenStrorageExpireTime != null && (tokenStrorageExpireTime == DateTime.MinValue || tokenStrorageExpireTime == DateTime.MaxValue)))
                    {
                        if (tokenResponse.expires != DateTime.MinValue && tokenResponse.expires != DateTime.MaxValue)
                        {
                            tokenStrorageExpireTime = tokenResponse.expires.AddMinutes(10);
                        }
                        else
                        {
                            tokenStrorageExpireTime = null;
                        }
                    }
                    else
                    {
                        if (tokenResponse.expires != DateTime.MinValue && tokenResponse.expires != DateTime.MaxValue)
                        {
                            if (tokenStrorageExpireTime < tokenResponse.expires)
                            {
                                tokenStrorageExpireTime = tokenResponse.expires.AddMinutes(10);
                            }
                        }
                    }

                    AccessTokenService.SetObject(tokenStorageKey, tokenResponse, tokenStrorageExpireTime);
                }
            }


            return tokenResponse;

        }
        
        public override void ChangeApiSettingsKey(string apiKey)
        {
            base.ChangeApiSettingsKey(apiKey);

            InitializeAuthenticatedClient(apiKey);
        }

        private void InitializeAuthenticatedClient(string apiKey)
        {
            apiConstantKey = ApiConstants.Api_Keys.API_ACCESS_TOKEN;
            if (!string.IsNullOrEmpty(apiKey))
            {
                apiConstantKey = apiConstantKey + "_" + apiKey;
            }

            tokenStorageKey = tokenStorageKey ?? apiConstantKey;

            if (ApiConfiguration != null && ApiConfiguration.Authentication != null && ApiConfiguration.Authentication.Enabled)
            {
                authenticationInfo = authenticationInfo ?? new AuthenticationInfo();

                authenticationInfo.UserName = ApiConfiguration.Authentication.Username;
                authenticationInfo.Password = ApiConfiguration.Authentication.Password;


                this.AuthenticationController = ApiConfiguration.Authentication.TokenPath;
                this.AuthenticationBaseUrl = ApiConfiguration.Authentication.BaseAddress;
                this.AuthenticationRefreshController = ApiConfiguration.Authentication.RefreshTokenPath;
            }
        }

        public override void AddAuthenticationRoute(string route)
        {
            if ((!route.StartsWith("/") && AuthenticationBaseUrl.EndsWith("/")) || (route.StartsWith("/") && !AuthenticationBaseUrl.EndsWith("/")))
            {
                AuthenticationBaseUrl += route;
            }
            else
            {
                route = "/" + route;
                AuthenticationBaseUrl += route;
            }
        }
    }
}
