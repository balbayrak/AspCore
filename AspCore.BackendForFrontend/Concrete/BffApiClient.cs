using AspCore.ApiClient;
using AspCore.ApiClient.Entities;
using AspCore.BackendForFrontend.Abstract;

namespace AspCore.BackendForFrontend.Concrete
{
    public class BffApiClient : JWTAuthenticatedApiClient<ApiClientConfiguration>, IBffApiClient
    {
        public BffApiClient(string apiKey) : base(apiKey)
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
