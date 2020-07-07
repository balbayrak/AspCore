using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using AspCore.ConfigurationAccess.Abstract;

namespace AspCore.ConfigurationAccess.Configuration
{
    public class ConfigurationManagerOption
    {
        public ConfigurationManagerOption()
        {
            type = EnumConfigurationAccessorType.None;
        }
        public EnumConfigurationAccessorType type { get; set; }

        public void AddConfigurationManager<T>(IServiceCollection services, Action<ConfigurationManagerOption> option)
          where T : class, IConfigurationAccessor, new()
        {
            
            var configurationHelper = services.FirstOrDefault(d => d.ServiceType == typeof(IConfigurationAccessor));
            if (configurationHelper == null)
            {
                services.Remove(configurationHelper);
            }

            services.AddSingleton<IConfigurationAccessor, T>();
        }
    }
}
