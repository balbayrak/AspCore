using AspCore.Storage.Abstract;
using AspCore.Utilities;
using AspCore.Utilities.DataProtector;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspCore.Storage.Concrete
{
    public class CookieManager : ICookieService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private string _uniqueKey;
        private bool _secureCookie;
        public CookieManager(IHttpContextAccessor contextAccessor, bool secureCookie = false, Guid? uniqueKey=null)
        {
            _contextAccessor = contextAccessor;
            _uniqueKey = uniqueKey.HasValue ? uniqueKey.Value.ToString("N") : Guid.NewGuid().ToString("N");
            _secureCookie = secureCookie;
        }

        public T GetObject<T>(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                key = $"{_uniqueKey}_{key}";
                var list = _contextAccessor.HttpContext.Response.Cookies;

                if (_contextAccessor.HttpContext.Request.Cookies.ContainsKey(key))
                {
                    var unProtectJson = DataProtectorFactory.Instance.UnProtect(_contextAccessor.HttpContext.Request.Cookies[key].ToString());
                    return JsonConvert.DeserializeObject<T>(unProtectJson.UnCompressString());
                }
            }
            return default(T);
        }

        public void Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                key = $"{_uniqueKey}_{key}";
                if (_contextAccessor.HttpContext != null)
                    _contextAccessor.HttpContext.Response.Cookies.Delete(key);
            }
        }

        public void RemoveAll()
        {
            foreach (var cookie in _contextAccessor.HttpContext.Request.Cookies.Keys.Where(t => t.StartsWith(_uniqueKey)))
            {
                Remove(cookie);
            }
        }

        public bool SetObject<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null)
        {
            try
            {
                key = $"{_uniqueKey}_{key}";
                Remove(key);

                CookieOptions option = new CookieOptions();
                option.Expires = expires ?? DateTime.UtcNow.AddMinutes(30);
                option.HttpOnly = true;
                option.SameSite = sameSiteStrict.HasValue && !sameSiteStrict.Value ? SameSiteMode.Lax : SameSiteMode.Strict;
                option.IsEssential = true;
                option.Secure = _secureCookie;
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                string json = JsonConvert.SerializeObject(obj, Formatting.None, serializerSettings).CompressString();

                var protectJson = DataProtectorFactory.Instance.Protect(json);
                _contextAccessor.HttpContext.Response.Cookies.Append(key, protectJson, option);
                return true;
            }
            catch
            {
                //loglama
            }

            return false;
        }
    }
}
