using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspCore.Storage.Abstract;
using AspCore.Storage.Concrete.Storage;

namespace AspCore.Storage.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureCacheStorage(this IServiceCollection services, Action<StorageOption> option)
        {
            using (StorageOptionBuilder builder = new StorageOptionBuilder(services))
            {
                builder.AddStorage(option);
                return services;
            }
        }

        public static IServiceCollection ConfigureApiClientWithCustomStorage<TStorageService>(this IServiceCollection services, Action<StorageOption> option)
             where TStorageService : class, IStorage, new()
        {
            using (StorageOptionBuilder builder = new StorageOptionBuilder(services))
            {
                builder.AddStorage<TStorageService>(option);
                return services;
            }
        }
    }
}
