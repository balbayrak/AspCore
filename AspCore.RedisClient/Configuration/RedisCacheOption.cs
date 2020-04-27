using AspCore.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.RedisClient.Configuration
{
    public class RedisCacheOption : IConfigurationEntity
    {
        public string instanceName { get; set; }
        public string[] servers { get; set; }
        /// <summary>
        /// use for redis sentinels
        /// </summary>
        public string masterName { get; set; }
    }
}
