using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities;

namespace AspCore.ConnectedApi.Concrete
{
    public abstract class BaseConnectedApiManager
    {
        public abstract string apiKey { get; }

        protected readonly IApiClient _apiClient;

        public BaseConnectedApiManager()
        {
            _apiClient = ApiClientFactory.Instance.GetApiClient(apiKey);
        }
    }
}
