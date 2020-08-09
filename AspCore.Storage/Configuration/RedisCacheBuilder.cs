using AspCore.ConfigurationAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Storage.Configuration
{
    public class RedisCacheBuilder
    {
        public readonly RedisCacheOption cacheOption;
        private readonly IConfigurationAccessor _configurationAccessor;
        public RedisCacheBuilder(IConfigurationAccessor configurationAccessor,string configurationKey)
        {
            _configurationAccessor = configurationAccessor;
            cacheOption = _configurationAccessor.GetValueByKey<RedisCacheOption>(configurationKey);
        }

        public RedisCacheBuilder(RedisCacheOption cacheOption)
        {
            this.cacheOption = cacheOption;
        }
    }
}
