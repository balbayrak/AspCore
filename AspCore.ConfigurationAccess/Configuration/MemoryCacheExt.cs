﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace AspCore.ConfigurationAccess.Configuration
{
    public static class MemoryCacheExt
    {
        public static void SetValue<T>(this IMemoryCache cache, string key, T value)
        {
            cache.Set(key, JsonConvert.SerializeObject(value));
        }

        public static void SetValue<T>(this IMemoryCache cache, string key, T value, TimeSpan absoluteExpirationRelativeToNow)
        {
            cache.Set(key, JsonConvert.SerializeObject(value), absoluteExpirationRelativeToNow);
        }

        public static void SetValue<T>(this IMemoryCache cache, string key, T value, MemoryCacheEntryOptions options)
        {
            cache.Set(key, JsonConvert.SerializeObject(value), options);
        }

        public static T GetValue<T>(this IMemoryCache cache, string key)
        {
            object value;
            try
            {
                if (cache.TryGetValue(key, out value))
                {
                    return JsonConvert.DeserializeObject<T>(value.ToString());
                }
            }
            catch
            {

            }
            finally
            {
                value = null;
            }
            return default(T);
        }
    }
}

