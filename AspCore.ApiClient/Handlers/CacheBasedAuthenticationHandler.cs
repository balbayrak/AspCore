using AspCore.ApiClient.Entities;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.General;
using AspCore.Storage.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace AspCore.ApiClient.Handlers
{
    public class CacheBasedAuthenticationHandler<TOption> : AspCoreAuthenticationHandler<TOption>
          where TOption : class, IApiClientConfiguration, new()
    {
        protected readonly ICacheService CacheService;

        public CacheBasedAuthenticationHandler(IServiceProvider serviceProvider, string apikey) : base(serviceProvider, apikey)
        {
            CacheService = ServiceProvider.GetRequiredService<ICacheService>();
        }

        public override async Task<AuthenticationTicketInfo> GetToken(HttpRequestMessage request = null, bool forceNewToken = false)
        {
            AuthenticationTicketInfo authenticationTicketInfo = await CacheService.GetObjectAsync<AuthenticationTicketInfo>(ApiKey);

            if (!forceNewToken && authenticationTicketInfo != null) return authenticationTicketInfo;

            AuthenticationInfo authenticationInfo = GetAuthenticationInfo();

            JsonContent jsonContent = new JsonContent(authenticationInfo);

            var response = await TokenClient.PostAsync($"/{ ConfigurationOption.Authentication.TokenPath.TrimStart('/')}", jsonContent);
            ServiceResult<AuthenticationTicketInfo> result = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseString = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<ServiceResult<AuthenticationTicketInfo>>(responseString);

                await AddorEditTokenStorage(result.Result);

                return result.Result;
            }

            if (result != null && !result.IsSucceeded)
            {
                throw new Exception($"{result.ErrorMessage} exception : {result.ExceptionMessage}");
            }

            throw new Exception(ApiConstants.Api_Keys.AUTHENTICATION_TOKEN_ERROR);
        }

        public override async Task<AuthenticationTicketInfo> RefreshToken(AuthenticationTicketInfo authenticationTicketInfo)
        {
            JsonContent jsonContent = new JsonContent(authenticationTicketInfo);
            var response = await TokenClient.PostAsync("/" + ConfigurationOption.Authentication.RefreshTokenPath, jsonContent);
            ServiceResult<AuthenticationTicketInfo> result = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseString = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<ServiceResult<AuthenticationTicketInfo>>(responseString);

                return result.Result;
            }

            if (result != null && !result.IsSucceeded)
            {
                throw new Exception($"{result.ErrorMessage} exception : {result.ExceptionMessage}");
            }


            throw new Exception($"");

        }

        public override async Task AddorEditTokenStorage(AuthenticationTicketInfo authenticationTicketInfo)
        {
            await CacheService.SetObjectAsync(ApiKey, authenticationTicketInfo, authenticationTicketInfo.expires.AddMinutes(-1));
        }

        public virtual AuthenticationInfo GetAuthenticationInfo()
        {
            AuthenticationInfo authenticationInfo = new AuthenticationInfo();

            authenticationInfo.UserName = ConfigurationOption.Authentication.Username;
            authenticationInfo.Password = ConfigurationOption.Authentication.Password;

            return authenticationInfo;

        }
    }
}
