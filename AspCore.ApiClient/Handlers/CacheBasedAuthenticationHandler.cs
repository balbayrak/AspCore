using AspCore.ApiClient.Entities;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.General;
using AspCore.Storage.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;


namespace AspCore.ApiClient.Handlers
{
    public class CacheBasedAuthenticationHandler<TOption> : AspCoreAuthenticationHandler<TOption>
          where TOption : class, IApiClientConfiguration, new()
    {
        private readonly ICacheService _cacheService;
        private readonly string _apikey;

        public CacheBasedAuthenticationHandler(IServiceProvider serviceProvider, string apikey) : base(serviceProvider, apikey)
        {
            _cacheService = ServiceProvider.GetRequiredService<ICacheService>();
            _apikey = apikey;
        }

        public override async Task<AuthenticationTicketInfo> GetToken()
        {
            AuthenticationTicketInfo authenticationTicketInfo = await _cacheService.GetObjectAsync<AuthenticationTicketInfo>(_apikey);

            if (authenticationTicketInfo != null) return authenticationTicketInfo;


            AuthenticationInfo authenticationInfo = new AuthenticationInfo();

            authenticationInfo.UserName = _configurationOption.Authentication.Username;
            authenticationInfo.Password = _configurationOption.Authentication.Password;

            JsonContent jsonContent = new JsonContent(authenticationInfo);


            var response = await _tokenClient.PostAsync($"/{ _configurationOption.Authentication.TokenPath.TrimStart('/')}", jsonContent);
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
            var response = await _tokenClient.PostAsync("/" + _configurationOption.Authentication.RefreshTokenPath, jsonContent);
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
            await _cacheService.SetObjectAsync<AuthenticationTicketInfo>(_apikey, authenticationTicketInfo);
        }
    }
}
