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


            if (managerOption.type == EnumConfigurationAccessorType.AppSettingJson)
            {
                _services.AddSingleton<IConfigurationAccessor, ConfigurationAccessor>();
            }
            else if (managerOption.type == EnumConfigurationAccessorType.DataBaseRemoteProvider)
            {

            }
            else if (managerOption.type == EnumConfigurationAccessorType.RedisRemoteProvider)
            {

            }
        }

        public void AddConfigurationHelper<T>(Action<ConfigurationManagerOption> option)
           where T : class, IConfigurationAccessor, new()
        {
            var configurationHelper = _services.FirstOrDefault(d => d.ServiceType == typeof(IConfigurationAccessor));
            if (configurationHelper == null)
            {
                _services.Remove(configurationHelper);
            }

            _services.AddSingleton<IConfigurationAccessor, T>();
        }

        public void Dispose()
        {

        }
    }
}
