using AspCore.ApiClient.Entities.Abstract;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.General;
using System;
using System.Net.Http;

namespace AspCore.ApiClient
{
    public class JWTAuthenticatedApiClient<TOption> : AuthenticatedApiClient<AuthenticationToken, TOption>
           where TOption : class, IApiClientConfiguration, new()
    {
        public JWTAuthenticatedApiClient(string apiKey) : base(apiKey)
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

            ServiceResult<AuthenticationToken> response = null;
            if (refreshToken)
            {
                AuthenticationToken token = _accessTokenService.GetObject<AuthenticationToken>(tokenStorageKey);
                if (token != null)
                {
                    this.apiUrl = AuthenticationRefreshController;
                    response = this.PostRequest<ServiceResult<AuthenticationToken>>(token).Result;
                }
            }
            else
            {
                this.apiUrl = AuthenticationController;
                response = this.PostRequest<ServiceResult<AuthenticationToken>>(input).Result;
            }


            this.baseAddress = oldApiBaseAddress;
            this.apiUrl = oldApiUrl;

            return response.Result;

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
            return tokenResponse;
        }

        public override AuthenticationToken GetTokenResponse(AuthenticationToken outputObj)
        {
            return outputObj;
        }


    }
}
