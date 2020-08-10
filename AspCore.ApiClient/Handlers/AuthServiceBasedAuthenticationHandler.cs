using AspCore.ApiClient.Entities;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.General;
using AspCore.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspCore.ApiClient.Handlers
{
    public class AuthServiceBasedAuthenticationHandler<TOption> : AspCoreAuthenticationHandler<TOption>
          where TOption : class, IApiClientConfiguration, new()
    {
        protected readonly IAuthenticationService AuthenticationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthServiceBasedAuthenticationHandler(IServiceProvider serviceProvider, string apiKey) : base(serviceProvider, apiKey)
        {
            AuthenticationService = ServiceProvider.GetRequiredService<IAuthenticationService>();
            _httpContextAccessor = ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        }

        public override async Task<AuthenticationTicketInfo> GetToken(HttpRequestMessage request = null, bool forceNewToken = false)
        {
            var accesToken = await _httpContextAccessor.HttpContext.GetTokenAsync(ApiConstants.Api_Keys.ACCESS_TOKEN);
            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(ApiConstants.Api_Keys.REFRESH_TOKEN);
            var expire = await _httpContextAccessor.HttpContext.GetTokenAsync(ApiConstants.Api_Keys.EXPIRES);

            AuthenticationTicketInfo authenticationTicketInfo = null;

            if (!string.IsNullOrEmpty(accesToken))
            {
                authenticationTicketInfo = new AuthenticationTicketInfo();
                authenticationTicketInfo.access_token = accesToken.UnCompressString();
                authenticationTicketInfo.refresh_token = refreshToken.UnCompressString();
                authenticationTicketInfo.expires = Convert.ToDateTime(expire);
            }

            return authenticationTicketInfo;
        }

        public override async Task<AuthenticationTicketInfo> RefreshToken(AuthenticationTicketInfo authenticationTicketInfo)
        {

            JsonContent jsonContent = new JsonContent(authenticationTicketInfo);
            var response = await _tokenClient.PostAsync("/" + _configurationOption.Authentication.RefreshTokenPath, jsonContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseString = await response.Content.ReadAsStringAsync();

                ServiceResult<AuthenticationTicketInfo> result = JsonConvert.DeserializeObject<ServiceResult<AuthenticationTicketInfo>>(responseString);

                return result.Result;
            }

            //var errorMessage = await GetErrorMessageAsync(response);
            throw new Exception(response.ToString());

        }

        public override async Task AddorEditTokenStorage(AuthenticationTicketInfo authenticationTicketInfo)
        {
            AuthenticateResult authenticateResult = await AuthenticationService.AuthenticateAsync(_httpContextAccessor.HttpContext, null);
            AuthenticationProperties properties = authenticateResult.Properties;

            properties.UpdateTokenValue(ApiConstants.Api_Keys.ACCESS_TOKEN, authenticationTicketInfo.access_token.CompressString());
            properties.UpdateTokenValue(ApiConstants.Api_Keys.REFRESH_TOKEN, authenticationTicketInfo.refresh_token.CompressString());
            properties.UpdateTokenValue(ApiConstants.Api_Keys.EXPIRES, authenticationTicketInfo.expires.ToString("o", CultureInfo.InvariantCulture));

            await AuthenticationService.SignInAsync(_httpContextAccessor.HttpContext, null, authenticateResult.Principal, authenticateResult.Properties);
        }

    }
}
