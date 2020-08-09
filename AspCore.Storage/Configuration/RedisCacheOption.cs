using AspCore.Entities.Configuration;

namespace AspCore.Storage.Configuration
{
    public class RedisCacheOption : StorageOption, IConfigurationEntity
    {
        public string instanceName { get; set; }
        public string[] servers { get; set; }
        /// <summary>
        /// use for redis sentinels
        /// </summary>
        public string masterName { get; set; }
    }
}
