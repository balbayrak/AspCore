using Microsoft.Extensions.Configuration;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.ConfigurationAccess.Configuration;
using AspCore.Entities.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace AspCore.ConfigurationAccess.Concrete
{
    public class ConfigurationAccessor : IConfigurationAccessor
    {
        private readonly IMemoryCache _memoryCache;
        public IConfiguration Configuration { get; }
        
        public ConfigurationAccessor()
        {

        }
        public ConfigurationAccessor(IConfiguration configuration,IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            Configuration = configuration;
        }
        public string GetConnectionStrings(string key)
        {
            var cacheValue = _memoryCache.GetValue<string>(key);
            if (string.IsNullOrEmpty(cacheValue))
            {
                var value=Configuration.GetConnectionString(key);
                _memoryCache.SetValue(key,value);
                return value;
            }
            return cacheValue;
        }
        public T GetValueByKey<T>(string key) where T : class, IConfigurationEntity, new()
        {
            var cacheValue = _memoryCache.GetValue<T>(key);
            if (cacheValue == null)
            {
                var value = new T();
                var section = Configuration.GetSection(key);
                section.Bind(value);
                _memoryCache.SetValue(key, value);
                return value;
            }
            return cacheValue;

        }
        public string GetSectionValue(string sectionName, string propName)
        {
            var key = $"{sectionName}:{propName}";
            var cacheValue = _memoryCache.GetValue<string>(key);
            if (string.IsNullOrEmpty(cacheValue))
            {
                var value = Configuration.GetValue<string>(key);
                _memoryCache.SetValue(key, value);
                return value;
            }
            return cacheValue;
         
        }
    }
}
