using AspCore.Caching.Abstract;
using AspCore.Utilities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Caching.Concrete
{
    public class CookieCacheManager : CacheManager, ICacheService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CookieCacheManager(IHttpContextAccessor contextAccessor) : base()
        {
            _contextAccessor = contextAccessor;
        }

        public bool ExpireEntryIn(string key, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        public T GetObject<T>(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                key = $"{uniqueCacheKey}_{key}";
                var list = _contextAccessor.HttpContext.Response.Cookies;

                if (_contextAccessor.HttpContext.Request.Cookies.ContainsKey(key))
                {
                    return JsonConvert.DeserializeObject<T>(_contextAccessor.HttpContext.Request.Cookies[key].ToString().UnCompressString());
                }
            }
            return default(T);
        }

        public bool Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                key = $"{uniqueCacheKey}_{key}";
                _contextAccessor.HttpContext.Response.Cookies.Delete(key);
                return true;
            }
            return false;

        }

        public void RemoveAll()
        {
            foreach (var cookie in _contextAccessor.HttpContext.Request.Cookies.Keys)
            {
                Remove(cookie);
            }
        }

        public bool SetObject<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null)
        {
            key = $"{uniqueCacheKey}_{key}";
            Remove(key);

            CookieOptions option = new CookieOptions();
            option.Expires = expires ?? DateTime.UtcNow.AddMinutes(30);
            option.HttpOnly = true;
            option.SameSite = sameSiteStrict.HasValue && !sameSiteStrict.Value ? SameSiteMode.Lax : SameSiteMode.Strict;
            option.IsEssential = true;
            //  option.Secure = true;
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.NullValueHandling = NullValueHandling.Ignore;
            string json = JsonConvert.SerializeObject(obj, Formatting.None, serializerSettings).CompressString();
            _contextAccessor.HttpContext.Response.Cookies.Append(key, json, option);
            return true;
        }
    }
}
