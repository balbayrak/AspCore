using AspCore.Caching.Abstract;

namespace AspCore.WebComponents.ViewComponents.Alert.Abstract
{
    public interface IAlertStorage : ICacheService
    {
        void Keep(string key);
    }
}
