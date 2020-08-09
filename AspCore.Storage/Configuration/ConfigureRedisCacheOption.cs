using AspCore.ConfigurationAccess.Abstract;
using AspCore.ConfigurationAccess.Concrete;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Storage.Configuration
{
    public class ConfigureRedisCacheOption : IConfigureNamedOptions<RedisCacheOptions>
    {
        private readonly RedisCacheBuilder _builder;
        public ConfigureRedisCacheOption(RedisCacheBuilder builder)
        {
            _builder = builder;
        }
        public void Configure(string name, RedisCacheOptions options)
        {
            RedisCacheOption redisCacheOption = _builder.cacheOption;

            if (redisCacheOption != null)
            {
                options.InstanceName = redisCacheOption.instanceName;

                if (redisCacheOption.servers != null && redisCacheOption.servers.Length > 1)
                {
                    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions();
                    options.ConfigurationOptions.ServiceName = redisCacheOption.masterName;
                    options.ConfigurationOptions.AbortOnConnectFail = false;
                    
                    foreach (var server in redisCacheOption.servers)
                    {
                        if (!string.IsNullOrEmpty(server))
                            options.ConfigurationOptions.EndPoints.Add(server);
                    }
                }
                else
                {
                    options.Configuration = redisCacheOption.servers[0];
                }
            }
        }

        public void Configure(RedisCacheOptions options) => Configure(Options.DefaultName, options);



    }
}

