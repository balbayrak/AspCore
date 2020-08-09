using AspCore.ApiClient;
using AspCore.ApiClient.Entities;
using AspCore.BackendForFrontend.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using System.Net.Http;

namespace AspCore.BackendForFrontend.Concrete
{
    public class BffApiClient : ApiClient<ApiClientConfiguration>, IBffApiClient
    {
        public BffApiClient(IHttpClientFactory httpClientFactory, IConfigurationAccessor configurationAccessor,  string apiKey) : base(httpClientFactory, configurationAccessor, apiKey)
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
