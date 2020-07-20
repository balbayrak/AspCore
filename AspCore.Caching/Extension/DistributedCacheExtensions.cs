using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace AspCore.Caching.Extension
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T> GetValueAsync<T>(this IDistributedCache cache, string key)
        {
            try
            {
                var data = await cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(data))
                {
                    return JsonConvert.DeserializeObject<T>(data);
                }
            }
            catch
            {

            }
            return default(T);
        }

        public static async Task<bool> SetValueAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options = null)
        {
             await cache.SetStringAsync(key, JsonConvert.SerializeObject(value),options);
             return true;
        }

        public static  T GetValue<T>(this IDistributedCache cache, string key)
        {
            try
            {
                var data =  cache.GetString(key);
                if (!string.IsNullOrEmpty(data))
                {
                    return JsonConvert.DeserializeObject<T>(data);
                }
            }
            catch
            {

            }
            return default(T);
        }

        public static void SetValue<T>(this IDistributedCache cache, string key, T value,DistributedCacheEntryOptions options=null)
        {
             cache.SetString(key, JsonConvert.SerializeObject(value),options);
        }
    }
}
