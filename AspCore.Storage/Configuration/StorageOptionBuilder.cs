using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Configuration;
using AspCore.Storage.Abstract;
using AspCore.Storage.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace AspCore.Storage.Configuration
{
    public class StorageOptionBuilder : CacheOptionBuilder
    {
        public StorageOptionBuilder(IServiceCollection services) : base(services)
        {
        }

        public void AddCookie(Action<CookieOption> option = null)
        {
            CookieOption cookieOption = null;

            option?.Invoke(cookieOption);

            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            services.AddSingleton<ICookieService>(sp =>
            {
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

                var secureCookie = cookieOption == null ? false : cookieOption.secureCookie;
                Guid? uniqueKey = cookieOption == null ? null : cookieOption.uniqueKey;

                return new CookieManager(httpContextAccessor, secureCookie, uniqueKey);
            });
        }
    }
}
