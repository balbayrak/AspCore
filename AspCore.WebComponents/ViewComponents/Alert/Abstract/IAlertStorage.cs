using AspCore.Storage.Abstract;

namespace AspCore.WebComponents.ViewComponents.Alert.Abstract
{
    public interface IAlertStorage 
    {
        void Keep(string key);

        T GetObject<T>(string key);

        bool Remove(string key);

        bool SetObject<T>(string key, T obj);
    }
}
