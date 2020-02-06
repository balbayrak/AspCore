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
            enumConfigurationManager = EnumConfigurationManager.None;
        }
        public EnumConfigurationManager enumConfigurationManager { get; set; }

        public void AddConfigurationManager<T>(IServiceCollection services, Action<ConfigurationManagerOption> option)
          where T : class, IConfigurationHelper, new()
        {
            var configurationHelper = services.FirstOrDefault(d => d.ServiceType == typeof(IConfigurationHelper));
            if (configurationHelper == null)
            {
                services.Remove(configurationHelper);
            }

            services.AddSingleton<IConfigurationHelper, T>();
        }
    }
}
