using AspCore.ApiClient.Abstract;
using AspCore.Dependency.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.ConnectedApi.Concrete
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
