using AspCore.ApiClient.Abstract;
using AspCore.Dependency.Concrete;
using System;

namespace AspCore.ApiClient.Entities.Concrete
{
    public class ApiClientFactory
    {
        private static ApiClientFactory _resolver;

        public static ApiClientFactory Instance
        {
            get
            {
                if (_resolver == null)
                    throw new Exception("ApiClientFactory not initialized. You should initialize it in Startup class");
                return _resolver;
            }
        }

        private ApiClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public static void Init(IServiceProvider services)
        {
            if (_resolver == null)
                _resolver = new ApiClientFactory(services);
        }

        private readonly IServiceProvider _serviceProvider;

        public IAuthenticatedApiClient GetApiClient(string apiKey)
        {
           return _serviceProvider.GetServiceByName<IAuthenticatedApiClient>(apiKey);
        }
    }
}
