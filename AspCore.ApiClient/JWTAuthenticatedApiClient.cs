using System;
using System.Net.Http;
using AspCore.ApiClient.Entities.Abstract;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Authentication.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.General;

namespace AspCore.ApiClient
{
    public class JWTAuthenticatedApiClient<TOption> : AuthenticatedApiClient<AuthenticationTokenResponse, TOption>
           where TOption : class, IApiClientConfiguration, new()
    {
        public JWTAuthenticatedApiClient(string apiKey) : base(apiKey)
        {

        }

        public override string AuthenticationBaseUrl { get; set; }
        public override string AuthenticationController { get; set; }

        public override string AuthenticationRefreshController { get; set; }

        public override AuthenticationTokenResponse AuthenticateClient(AuthenticationInfo input, Func<AuthenticationTokenResponse, AuthenticationTokenResponse> func, bool forceAuthentication, bool refreshToken)
        {
            string oldApiBaseAddress = this.baseAddress;
            string oldApiUrl = this.apiUrl;

            this.baseAddress = AuthenticationBaseUrl;

            ServiceResult<AuthenticationTokenResponse> response = null;
            if (refreshToken)
            {
                AuthenticationTokenResponse token = _accessTokenService.GetObject<AuthenticationTokenResponse>(tokenStorageKey);
                if (token != null)
                {
                    this.apiUrl = AuthenticationRefreshController;
                    response = this.PostRequest<ServiceResult<AuthenticationTokenResponse>>(token).Result;
                }
            }
            else
            {
                this.apiUrl = AuthenticationController;
                response = this.PostRequest<ServiceResult<AuthenticationTokenResponse>>(input).Result;
            }


            this.baseAddress = oldApiBaseAddress;
            this.apiUrl = oldApiUrl;

            return response.Result;

        }

        public override AuthenticationTokenResponse Authenticate(HttpClient client, bool forceAuthentication, bool refreshToken)
        {
            AuthenticationTokenResponse tokenResponse = base.Authenticate(client, forceAuthentication, refreshToken);

            if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.access_token))
            {
                if (!tokenResponse.access_token.Contains(ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER))
                    tokenResponse.access_token = ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER + " " + tokenResponse.access_token;

                client.DefaultRequestHeaders.Remove(ApiConstants.Api_Keys.API_AUTHORIZATION);
                client.DefaultRequestHeaders.Add(ApiConstants.Api_Keys.API_AUTHORIZATION, tokenResponse.access_token);
            }
            return tokenResponse;
        }

        public override AuthenticationTokenResponse GetTokenResponse(AuthenticationTokenResponse outputObj)
        {
            return outputObj;
        }


    }
}
