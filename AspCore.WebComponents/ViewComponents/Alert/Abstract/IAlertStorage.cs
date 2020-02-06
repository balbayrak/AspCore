using AspCore.Storage.Abstract;

namespace AspCore.WebComponents.ViewComponents.Alert.Abstract
{
    public interface IAlertStorage : IStorage
    {
        void Keep(string key);
    }
}
