using AspCore.ApiClient.Abstract;
using AspCore.Dependency.Concrete;

namespace AspCore.Business.Manager
{
    public abstract class BaseConnectedApiManager
    {
        public abstract string apiKey { get; }

        protected readonly IApiClient _apiClient;

        public BaseConnectedApiManager()
        {
            _apiClient = DependencyResolver.Current.GetServiceByName<IAuthenticatedApiClient>(apiKey);
        }
    }
}
