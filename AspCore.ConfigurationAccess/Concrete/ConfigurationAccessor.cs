using Microsoft.Extensions.Configuration;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Configuration;

namespace AspCore.ConfigurationAccess.Concrete
{
    public class ConfigurationAccessor : IConfigurationAccessor
    {
        public IConfiguration Configuration { get; }

        public ConfigurationAccessor()
        {

        }
        public ConfigurationAccessor(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string GetConnectionStrings(string key)
        {
            return Configuration.GetConnectionString(key);
        }
        public T GetValueByKey<T>(string key) where T : class, IConfigurationEntity, new()
        {
            var value = new T();
            var section = Configuration.GetSection(key);
            section.Bind(value);
            return value;
        }
        public string GetSectionValue(string sectionName, string propName)
        {
            return Configuration.GetValue<string>($"{sectionName}:{propName}");
        }
    }
}
