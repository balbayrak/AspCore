using AspCore.Entities.Configuration;
using AspCore.Storage.Abstract;
using AspCore.Storage.Concrete;
using AspCore.Storage.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace AspCore.Web.Configuration.Options
{
    public class StorageOptionConfiguration : ConfigurationOption
    {
        public StorageOptionConfiguration(IServiceCollection services) : base(services)
        {
        }

        public ApiClientConfigurationOption AddStorageService(Action<StorageOptionBuilder> option)
        {
            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }


            var storageOptionBuilder = new StorageOptionBuilder(services);
            option(storageOptionBuilder);


            services.AddSingleton<StorageService>(sp =>
            {
                var cacheService = sp.GetService<ICacheService>();
                var cookieService = sp.GetService<ICookieService>();

                return new StorageService(cookieService, cacheService);
            });

            return new ApiClientConfigurationOption(services);
        }
    }
}
