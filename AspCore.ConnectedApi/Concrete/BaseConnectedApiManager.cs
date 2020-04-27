using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities.Concrete;
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
            _apiClient = ApiClientFactory.Instance.GetApiClient(apiKey);
        }
    }
}
