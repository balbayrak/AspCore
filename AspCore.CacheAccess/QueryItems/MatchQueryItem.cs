﻿using AspCore.CacheAccess.QueryBuilder.Concrete;
using System;

namespace AspCore.CacheAccess.QueryItems
{
    /// <summary>
    /// Match Query aranan kelimeyi analiz eder, Büyük küçük harf farketmeden sonuç getirir.
    /// </summary>
  
    public class MatchQueryItem : QueryItem
    {
        public string Value { get; set; }

        public MatchQueryItem(string fieldDescriptor, string value) : base(fieldDescriptor)
        {
            this.Value = value;
        }
    }
}
