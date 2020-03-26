using AspCore.Caching.Abstract;
using AspCore.Caching.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace AspCore.ApiClient.Configuration
{
    public class ApiClientOption
    {
        public EnumCache cacheType { get; set; }

        public void AddStorageCustomService<T>(IServiceCollection services)
            where T : class, ICacheService, new()
        {
            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            var accessTokenService = services.FirstOrDefault(d => d.ServiceType == typeof(ICacheService));
            if (accessTokenService == null)
            {
                services.Remove(accessTokenService);
            }

            services.AddScoped<ICacheService, T>();
        }
    }
}
