using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using AspCore.Entities.Configuration;
using AspCore.Storage.Abstract;
using AspCore.Storage.Concrete.Storage;

namespace AspCore.Storage.Configuration
{
    public class StorageOptionBuilder : ConfigurationOption, IDisposable
    {
        public StorageOptionBuilder(IServiceCollection services) :base(services)
        {

        }
        public void AddStorage( Action<StorageOption> option)
        {
            StorageOption apiClientOption = new StorageOption();
            option.Invoke(apiClientOption);

            if (apiClientOption.storageType == EnumStorage.Cookie)
            {

                var httpContextAccessor = _services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
                if (httpContextAccessor == null)
                {
                    _services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                }

                _services.AddSingleton<IStorage, CookieStorage>();

            }
            else if (apiClientOption.storageType == EnumStorage.MemoryCache)
            {
                _services.AddSingleton<IStorage, MemCacheStorage>();
            }

        }

        public void AddStorage<T>(Action<StorageOption> option)
        where T : class, IStorage, new()
        {
            var httpContextAccessor = _services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                _services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            var accessTokenService = _services.FirstOrDefault(d => d.ServiceType == typeof(IStorage));
            if (accessTokenService == null)
            {
                _services.Remove(accessTokenService);
            }

            _services.AddSingleton<IStorage, T>();
        }

        public void Dispose()
        {

        }
    }
}
