using AspCore.ApiClient;
using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Caching.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace AspCore.BackendForFrontend.Concrete
{
    public class BffApiClient : JWTAuthenticatedApiClient<ApiClientConfiguration>, IBffApiClient
    {
        public BffApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfigurationAccessor configurationAccessor, ICacheService cacheService, ICancellationTokenHelper tokenHelper, string apiKey) : base(httpClientFactory, httpContextAccessor, configurationAccessor, cacheService, tokenHelper, apiKey)
        {
        }
        private string _apiClientKey { get; set; }
        public string apiClientKey
        {
            get => _apiClientKey;
            set
            {
                _apiClientKey = value;
                ChangeApiSettingsKey(value);
            }
        }
    }
}
