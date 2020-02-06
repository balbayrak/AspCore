using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.ConfigurationAccess.Concrete;
using AspCore.Entities.Configuration;

namespace AspCore.ConfigurationAccess.Configuration
{
    public class ConfigurationManagerOptionBuilder : ConfigurationOption, IDisposable
    {
        public ConfigurationManagerOptionBuilder(IServiceCollection services) : base(services)
        {

        }
        public void AddConfigurationHelper(Action<ConfigurationManagerOption> option)
        {
            ConfigurationManagerOption managerOption = new ConfigurationManagerOption();
            option.Invoke(managerOption);


            if (managerOption.enumConfigurationManager == EnumConfigurationManager.AppSettingJson)
            {
                _services.AddSingleton<IConfigurationHelper, ConfigurationHelper>();
            }
            else if (managerOption.enumConfigurationManager == EnumConfigurationManager.DataBaseRemoteProvider)
            {

            }
            else if (managerOption.enumConfigurationManager == EnumConfigurationManager.RedisRemoteProvider)
            {

            }
        }

        public void AddConfigurationHelper<T>(Action<ConfigurationManagerOption> option)
           where T : class, IConfigurationHelper, new()
        {
            var configurationHelper = _services.FirstOrDefault(d => d.ServiceType == typeof(IConfigurationHelper));
            if (configurationHelper == null)
            {
                _services.Remove(configurationHelper);
            }

            _services.AddSingleton<IConfigurationHelper, T>();
        }

        public void Dispose()
        {

        }
    }
}
