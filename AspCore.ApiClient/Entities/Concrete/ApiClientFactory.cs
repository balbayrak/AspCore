using System;
using System.Collections.Generic;
using System.Text;
using AspCore.ApiClient.Abstract;
using AspCore.Dependency.Concrete;

namespace AspCore.ApiClient.Entities.Concrete
{
    public class ApiClientFactory
    {
        public static IAuthenticatedApiClient GetApiClient(string apiKey)
        {
           return  DependencyResolver.Current.GetServiceByName<IAuthenticatedApiClient>(apiKey);
        }
    }
}
