using System;
using AspCore.WebComponents.HtmlHelpers.Extensions;
using AspCore.WebComponents.ViewComponents.Alert.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace AspCore.WebComponents.ViewComponents.Alert.Concrete.Alert
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

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }
    }
}
