using AspCore.ApiClient;
using AspCore.ApiClient.Entities;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Caching.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using Microsoft.AspNetCore.Http;

namespace AspCore.BackendForFrontend.Concrete
{
    public class BffApiClient : JWTAuthenticatedApiClient<ApiClientConfiguration>, IBffApiClient
    {
        public BffApiClient(IHttpContextAccessor httpContextAccessor, IConfigurationAccessor configurationAccessor, ICacheService cacheService, string apiKey) : base(httpContextAccessor, configurationAccessor, cacheService, apiKey)
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
