using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace AspCore.Utilities.DataProtector
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDataProtector(this IServiceCollection services, Action<DataProtectorOption> option)
        {
            var dataProtectorOption = new DataProtectorOption();
            option(dataProtectorOption);

            if (!string.IsNullOrEmpty(dataProtectorOption.dataProtectorKey))
            {
                services.AddDataProtection()
                     .PersistKeysToFileSystem(new DirectoryInfo($"{dataProtectorOption.persistFileSytemPath}"))
                     .ProtectKeysWithDpapi()
                     .SetDefaultKeyLifetime(TimeSpan.FromDays(dataProtectorOption.lifeTime));

                services.AddSingleton(typeof(IDataProtectorHelper), sp =>
                {
                    var dataProtectionProvider = sp.GetRequiredService<IDataProtectionProvider>();
                    return new DataProtectorHelper(dataProtectionProvider, dataProtectorOption.dataProtectorKey);
                });
            }

            return services;
        }
    }
}
