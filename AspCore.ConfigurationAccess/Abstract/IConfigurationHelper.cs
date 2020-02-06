
using AspCore.Dependency.Abstract;
using AspCore.Entities.Configuration;

namespace AspCore.ConfigurationAccess.Abstract
{
    public interface IConfigurationHelper : ISingletonType
    {
        string GetConnectionStrings(string key);
        T GetValueByKey<T>(string key) where T : class, IConfigurationEntity, new();
        string GetSectionValue(string sectionName, string propName);

    }
}
