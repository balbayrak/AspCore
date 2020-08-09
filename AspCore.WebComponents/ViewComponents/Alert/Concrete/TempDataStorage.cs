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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ITempDataDictionary TempData => _tempDataDictionaryFactory?.GetTempData(_httpContextAccessor.HttpContext);

        public TempDataStorage(IHttpContextAccessor contextAccessor)
        {
            _httpContextAccessor = contextAccessor;
            _tempDataDictionaryFactory = (ITempDataDictionaryFactory)contextAccessor.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory));
            _serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
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

        public bool SetObject<T>(string key, T obj)
        {
            TempData[key] = JsonConvert.SerializeObject(obj);
            return true;
        }

    }
}
