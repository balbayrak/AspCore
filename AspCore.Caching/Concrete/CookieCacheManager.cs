using AspCore.Caching.Abstract;
using AspCore.Utilities;
using AspCore.Utilities.DataProtector;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspCore.Caching.Concrete
{
    public class CookieCacheManager : CacheManager, ICookieService
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

        public override T GetObject<T>(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                key = $"{uniqueCacheKey}_{key}";
                var list = _contextAccessor.HttpContext.Response.Cookies;

                if (_contextAccessor.HttpContext.Request.Cookies.ContainsKey(key))
                {
                    var unProtectJson = DataProtectorFactory.Instance.UnProtect(_contextAccessor.HttpContext.Request.Cookies[key].ToString());
                    return JsonConvert.DeserializeObject<T>(unProtectJson.UnCompressString());
                }
            }
            return default(T);
        }

        public override bool Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                //key = $"{uniqueCacheKey}_{key}";
                if (_contextAccessor.HttpContext != null)
                    _contextAccessor.HttpContext.Response.Cookies.Delete(key);
                return true;
            }
            return false;

        }

        public override bool RemoveAll()
        {
            foreach (var cookie in _contextAccessor.HttpContext.Request.Cookies.Keys.Where(t => t.StartsWith(uniqueCacheKey)))
            {
                Remove(cookie);
            }
            return true;
        }

        public override bool SetObject<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null)
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

            var protectJson = DataProtectorFactory.Instance.Protect(json);
            _contextAccessor.HttpContext.Response.Cookies.Append(key, protectJson, option);
            return true;
        }
    }
}
