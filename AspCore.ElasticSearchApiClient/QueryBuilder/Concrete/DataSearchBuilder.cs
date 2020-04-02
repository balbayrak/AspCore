﻿using AspCore.ElasticSearchApiClient.General;
using AspCore.ElasticSearchApiClient.QueryContiner.Concrete;
using System;
using System.Linq.Expressions;

namespace AspCore.ElasticSearchApiClient.QueryBuilder.Concrete
{
    public class DataSearchBuilder<T> where T : class
    {
        internal FilterSource sourceFilter;
        internal SortItem sortItem;
        internal string _IndexName;
        internal int _Size;
        internal int _From;
        internal string _IdFieldName;
        internal ComplexQueryItemContainer queryContainer;
        internal ComplexQueryItemContainer postFilterQueryContainer;
        internal QueryDescriptor<T> queryDescriptor;
        internal FilterSourceBuilder<T> filterSourceBuilder;
        internal Lazy<SortBuilder<T>> sortBuilder = new Lazy<SortBuilder<T>>(() => new SortBuilder<T>());

        public DataSearchBuilder()
        {
            queryDescriptor = new QueryDescriptor<T>();
            this._Size = 10;
            this._From = 0;
            queryContainer = null;
            postFilterQueryContainer = null;
            sortItem = null;
            sourceFilter = null;

        }

        /// <summary>
        /// Cache sunucusunda tanımlanan cache key değeridir.
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public DataSearchBuilder<T> IndexName(string indexName)
        {
            _IndexName = indexName;
            return this;
        }

        /// <summary>
        /// Cache Id field adıdır. Aggregation için kullanılır.
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public DataSearchBuilder<T> TotalCountAgg(Expression<Func<T, object>> idField)
        {
            _IdFieldName = idField.GetPropertyName();
            return this;
        }

        public DataSearchBuilder<T> MinMaxAgg(Expression<Func<T, object>> idField)
        {
            _IdFieldName = idField.GetPropertyName();
            return this;
        }

        /// <summary>
        /// Result edilecek item sayısıdır. -1 girilirse 10000 değer return edilir.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public DataSearchBuilder<T> Size(int size)
        {
            _Size = size == -1 ? 10000 : size;
            return this;
        }

        /// <summary>
        /// Sayfalama için kullanılır. Return edilecek değerin başlangıç index değerini ifade eder.
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public DataSearchBuilder<T> From(int from)
        {
            _From = from;
            return this;
        }

        /// <summary>
        /// Return edilecek property değerleri seçilebilir, default olarak include all seçilir.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public DataSearchBuilder<T> SourceFilter(params Func<FilterSourceBuilder<T>, FilterSourceBuilder<T>>[] filters)
        {
            filterSourceBuilder = filterSourceBuilder ?? new FilterSourceBuilder<T>();
            foreach (var filter in filters)
            {
                filterSourceBuilder = filter(filterSourceBuilder);
            }
            return this;
        }

        /// <summary>
        /// Sıralama için seçilen yön ve property değerleri seçilir.
        /// </summary>
        /// <param name="sortItems"></param>
        /// <returns></returns>
        public DataSearchBuilder<T> Sort(params Func<SortBuilder<T>, SortItem>[] sortItems)
        {
            foreach (var sort in sortItems)
            {
                sortItem = sort(sortBuilder.Value);
            }
            return this;
        }

        /// <summary>
        /// Arama yapılacak sorgu oluşturulur.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSearchBuilder<T> Query(Func<QueryDescriptor<T>, ComplexQueryItemContainer> query)
        {
            queryContainer = query(queryDescriptor);
            return this;
        }

        /// <summary>
        /// Arama yapılacak sorgu oluşturulur.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSearchBuilder<T> Query(ComplexQueryItemContainer query)
        {
            queryContainer = query;
            return this;
        }

        /// <summary>
        /// Filtreleme işlemi bittikten sonra çalışacak olan sorgudur. Filtreleme işleminden sonra çalışak ikinci bnir sorgudur.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSearchBuilder<T> PostFilter(Func<QueryDescriptor<T>, ComplexQueryItemContainer> query)
        {
            postFilterQueryContainer = query(queryDescriptor);
            return this;
        }

        public SearchRequestItem GetRequestItem()
        {
            return new SearchRequestItem
            {
                from = this._From,
                size = this._Size,
                queryContainer = this.queryContainer,
                postFilterQueryContainer = this.postFilterQueryContainer,
                sortItem = this.sortItem,
                sourceFilter = filterSourceBuilder != null ? filterSourceBuilder.filterSource : new FilterSource(true, false),
                IdFieldPropertyName = this._IdFieldName
            };
        }


    }
}
