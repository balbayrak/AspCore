﻿using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using System;

namespace AspCore.ElasticSearchApiClient.QueryItems
{
    /// <summary>
    /// Multi Match Query aranan ifadeyi belirtilen alanlarda analiz eder, Büyük küçük harf farketmeden sonuç getirir.
    /// </summary>
    public class MultiMatchQueryItem : QueryItem
    {
        public string[] fields { get; set; }
        public object Value { get; set; }

        public string MinimumShouldMatchPercentage { get; set; }

        public string MinimumShouldMatchFixed { get; set; }

        public EnumMultiQueryOperation? operation { get; set; }


        public MultiMatchQueryItem(string[] fields, object value, string minimumShouldMatchPercentage, string minimumShouldMatchFixed, EnumMultiQueryOperation? operation) : base()
        {
            this.fields = fields;
            this.Value = value;
            this.MinimumShouldMatchPercentage = minimumShouldMatchPercentage;
            this.MinimumShouldMatchFixed = minimumShouldMatchFixed;
            this.operation = operation;
        }
    }
}