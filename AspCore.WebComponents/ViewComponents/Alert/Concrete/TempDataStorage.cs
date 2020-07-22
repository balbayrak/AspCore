using System;
using System.Threading.Tasks;
using AspCore.Caching.Abstract;
using AspCore.WebComponents.HtmlHelpers.Extensions;
using AspCore.WebComponents.ViewComponents.Alert.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace AspCore.WebComponents.ViewComponents.Alert.Concrete
{
    public class TempDataStorage : IAlertStorage
    {
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        private ITempDataDictionary TempData => _tempDataDictionaryFactory?.GetTempData(HttpContextWrapper.Current);

        public TempDataStorage(IHttpContextAccessor contextAccessor)
        {
            _tempDataDictionaryFactory = (ITempDataDictionaryFactory)contextAccessor.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory));
            _serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        public bool ExpireEntryIn(string key, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        public T GetObject<T>(string key)
        {
            if (TempData.ContainsKey(key))
            {
                return JsonConvert.DeserializeObject<T>(TempData[key] as string);
            }
            return default(T);
        }

        public async Task<T> GetObjectAsync<T>(string key)
        {
            var data = await Task.Run(() => GetObject<T>(key));
            return data;
        }


        public async Task<bool> SetObjectAsync<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null)
        {
            var data = await Task.Run(() => SetObject(key, obj, expires, sameSiteStrict));
            return data;
        }

        public void Keep(string key)
        {
            TempData.Keep(key);
        }
        public bool Remove(string key)
        {
            return TempData.ContainsKey(key) && TempData.Remove(key);
        }

        public bool SetObject<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null)
        {
            TempData[key] = JsonConvert.SerializeObject(obj);
            return true;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            var data = await Task.Run(() => Remove(key));
            return data;
        }

        public async Task<bool> RemoveAllAsync()
        {
            var data = await Task.Run(() => RemoveAll());
            return data;
        }

        public bool RemoveAll()
        {
            return true;
        }
    }
}
