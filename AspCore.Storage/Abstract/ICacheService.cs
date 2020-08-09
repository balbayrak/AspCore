using System;
using System.Threading.Tasks;

namespace AspCore.Storage.Abstract
{
    public interface ICacheService
    {
        T GetObject<T>(string key);
      
        Task<T> GetObjectAsync<T>(string key);

        bool SetObject<T>(string key, T obj, DateTime? expires = null);
       
        Task<bool> SetObjectAsync<T>(string key, T obj, DateTime? expires = null);

        bool Remove(string key);

        Task<bool> RemoveAsync(string key);

        bool ExpireEntryIn(string key, TimeSpan timeSpan);
    }
}
