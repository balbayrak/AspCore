using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Storage.Abstract;
using AspCore.Storage.Concrete.Storage;

namespace AspCore.ApiClient.Configuration
{
    public class ApiClientOption
    {
        public EnumStorage tokenStorage { get; set; }

        public void AddStorageCustomService<T>(IServiceCollection services)
            where T : class, IStorage, new()
        {
            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            var accessTokenService = services.FirstOrDefault(d => d.ServiceType == typeof(IStorage));
            if (accessTokenService == null)
            {
                services.Remove(accessTokenService);
            }

            services.AddScoped<IStorage, T>();
        }
    }
}
