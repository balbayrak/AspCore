using AspCore.ApiClient.Entities.Abstract;
using AspCore.Caching.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using AspCore.ApiClient.Abstract;

namespace AspCore.ApiClient
{
    public class BearerAuthenticatedApiClient<TOption> : AuthenticatedApiClient<AuthenticationToken, TOption>
                 where TOption : class, IApiClientConfiguration, new()
    {
        public BearerAuthenticatedApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfigurationAccessor configurationAccessor, ICacheService cacheService, ICancellationTokenHelper cancellationTokenHelper, string apiKey) : base(httpClientFactory,httpContextAccessor, configurationAccessor, cacheService, cancellationTokenHelper,apiKey)
        {

        }

        public override string AuthenticationBaseUrl { get; set; }
        public override string AuthenticationController { get; set; }

        public override string AuthenticationRefreshController { get; set; }

        public override AuthenticationToken AuthenticateClient(AuthenticationInfo input, Func<AuthenticationToken, AuthenticationToken> func, bool forceAuthentication, bool refreshToken)
        {

            string oldApiBaseAddress = this.baseAddress;
            string oldApiUrl = this.apiUrl;

            this.baseAddress = AuthenticationBaseUrl;
            this.apiUrl = AuthenticationController;

            var pairs = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>(ApiConstants.Api_Keys.BEARER_TOKEN_GRANTTYPE, "password"),
                            new KeyValuePair<string, string>(ApiConstants.Api_Keys.BEARER_TOKEN_USERNAME, input.UserName ),
                            new KeyValuePair<string, string> (ApiConstants.Api_Keys.BEARER_TOKEN_PASSWORD, input.Password )
                        };
            var content = new FormUrlEncodedContent(pairs);


            AuthenticationToken response = this.PostRequest<AuthenticationToken>(content).Result;

            this.baseAddress = oldApiBaseAddress;
            this.apiUrl = oldApiUrl;

            return response;

        }

        public override AuthenticationToken Authenticate(HttpClient client, bool forceAuthentication, bool refreshToken)
        {
            AuthenticationToken tokenResponse = base.Authenticate(client, forceAuthentication, refreshToken);

            if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.access_token))
            {
                if (!tokenResponse.access_token.Contains(ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER))
                    tokenResponse.access_token = ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER + " " + tokenResponse.access_token;

                client.DefaultRequestHeaders.Remove(ApiConstants.Api_Keys.API_AUTHORIZATION);
                client.DefaultRequestHeaders.Add(ApiConstants.Api_Keys.API_AUTHORIZATION, tokenResponse.access_token);
            }

            return null;
        }

        public override AuthenticationToken GetTokenResponse(AuthenticationToken outputObj)
        {
            return outputObj;
        }

    }
}
