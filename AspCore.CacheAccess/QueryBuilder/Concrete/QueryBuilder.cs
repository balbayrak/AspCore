using AspCore.CacheAccess.QueryContiner.Abstract;
using AspCore.CacheAccess.QueryContiner.Concrete;
using AspCore.CacheAccess.QueryItems;
using AspCore.Extension;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TseCacheManagement.CacheAccess.General;

namespace AspCore.CacheAccess.QueryBuilder.Concrete
{
    public class QueryBuilder<T> where T : class
    {
        public QueryBuilder()
        {

        }

        /// <summary>
        /// Seçilen field için verilen değer araması yapılır. Değer analiz edilmeden arama yapılır.
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer TermQuery(Expression<Func<T, object>> fieldDescriptor, object value)
        {
            TermQueryItem query = new TermQueryItem(fieldDescriptor.GetPropertyName(), value);
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Verilen field adında değer araması yapılır. Değer analiz edilmeden arama yapılır.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer TermQuery(string field, object value)
        {
            TermQueryItem query = new TermQueryItem(field, value);
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçilen field için verilen değer araması yapılır. Değer analiz edilmeden arama yapılır.
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer TermsQuery(Expression<Func<T, object>> fieldDescriptor, IEnumerable<object> values)
        {
            TermsQueryItem query = new TermsQueryItem(fieldDescriptor.GetPropertyName(), values);
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Verilen field adında değer araması yapılır. Değer analiz edilmeden arama yapılır.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer TermsQuery(string field, IEnumerable<object> values)
        {
            TermsQueryItem query = new TermsQueryItem(field, values);
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçilen field için verilen değer ile başlayan ifadeler aranır. Değer analiz edilmeden arama yapılır.
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer PrefixQuery(Expression<Func<T, object>> fieldDescriptor, string value)
        {
            PrefixQueryItem query = new PrefixQueryItem(fieldDescriptor.GetPropertyName(), value.ConvertTurkishCharacter());
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Verilen field adı için verilen değer ile başlayan ifadeler aranır. Değer analiz edilmeden arama yapılır.
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer PrefixQuery(string field, string value)
        {
            PrefixQueryItem query = new PrefixQueryItem(field, value.ConvertTurkishCharacter());
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçilen field için değer araması yapılır. Değer analiz edilir.
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer MatchQuery(Expression<Func<T, object>> fieldDescriptor, string value)
        {
            MatchQueryItem query = new MatchQueryItem(fieldDescriptor.GetPropertyName(), value.ConvertTurkishCharacter());
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Index içindeki bütün veri getirilir.
        /// </summary>
        /// <returns></returns>
        public IQueryItemContainer MatchAllQuery()
        {
            MatchAllQueryItem query = new MatchAllQueryItem();
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Verilen field adında değer araması yapılır. Değer analiz edilir.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer MatchQuery(string field, string value)
        {
            MatchQueryItem query = new MatchQueryItem(field, value.ConvertTurkishCharacter());
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçilen field için cümle araması yapılır. Verilen cümle ile exact arama gerçekleştirilir.
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer MatchPhraseQuery(Expression<Func<T, object>> fieldDescriptor, string value)
        {
            MatchPhraseQueryItem query = new MatchPhraseQueryItem(fieldDescriptor.GetPropertyName(), value.ConvertTurkishCharacter());
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Verilen field adında cümle araması yapılır. Verilen kelimelere aynı sırada arama yapılır.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer MatchPhraseQuery(string field, string value)
        {
            MatchPhraseQueryItem query = new MatchPhraseQueryItem(field, value.ConvertTurkishCharacter());
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçilen field için cümle araması yapılır. Verilen kelimeler aynı sırada arama yapılır. Verilen değer ile başlayan ifadeler bulunur.
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer MatchPhrasePrefixQuery(Expression<Func<T, object>> fieldDescriptor, string value)
        {
            MatchPhrasePrefixQueryItem query = new MatchPhrasePrefixQueryItem(fieldDescriptor.GetPropertyName(), value.ConvertTurkishCharacter());
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Verilen field adında cümle araması yapılır. Verilen kelimeler aynı sırada arama yapılır. Verilen değer ile başlayan ifadeler bulunur.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer MatchPhrasePrefixQuery(string field, string value)
        {
            MatchPhrasePrefixQueryItem query = new MatchPhrasePrefixQueryItem(field, value.ConvertTurkishCharacter());
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçlien field için verilen tarih aralığında arama yapar. Seçilen alan DateTime formatında olmalıdır.
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <param name="lessThan"></param>
        /// <param name="greaterThan"></param>
        /// <returns></returns>
        public IQueryItemContainer DateRangeQuery(Expression<Func<T, object>> fieldDescriptor, DateTime? lessThan, DateTime? greaterThan)
        {
            DateRangeQueryItem query = new DateRangeQueryItem(fieldDescriptor.GetPropertyName(), lessThan, greaterThan);
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Verilen field adı için index içerisinde bulunup bulunmadığı araması yapılır.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IQueryItemContainer ExistQuery(string field)
        {
            ExistQueryItem query = new ExistQueryItem(field);
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçilen field için index içerisinde bulunup bulunmadığı araması yapılır.
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <returns></returns>
        public IQueryItemContainer ExistQuery(Expression<Func<T, object>> fieldDescriptor)
        {
            ExistQueryItem query = new ExistQueryItem(fieldDescriptor.GetPropertyName());
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçilen field üzerinde text ve yakın anlamlı text araması yapar.
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <param name="value"></param>
        /// <param name="prefixLength">Aranan kelimenin ilk olarak alıncanacek ve analize edilmeyecek olan kısmının boyutudur. Default olarak 0 alınır.</param>
        /// <param name="maxExpansions">Sorgunun yakın anlamalı kelime sınırını belirler.</param>
        /// <param name="transpositions">ab->ba gibi ters dönüşümlerde geçerli mi? Default false olarak alınır.</param>
        /// <returns></returns>
        public IQueryItemContainer FuzzyQuery(Expression<Func<T, object>> fieldDescriptor, string value, int? prefixLength, int? maxExpansions, bool? transpositions)
        {
            FuzzyQueryItem query = new FuzzyQueryItem(fieldDescriptor.GetPropertyName(), value.ConvertTurkishCharacter(), prefixLength, maxExpansions, transpositions);
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Verilen field adında  text ve yakın anlamlı text araması yapar.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="prefixLength">Aranan kelimenin ilk olarak alıncanacek ve analize edilmeyecek olan kısmının boyutudur. Default olarak 0 alınır.</param>
        /// <param name="maxExpansions">Sorgunun yakın anlamalı kelime sınırını belirler.</param>
        /// <param name="transpositions">ab->ba gibi ters dönüşümlerde geçerli mi? Default false olarak alınır.</param>
        /// <returns></returns>
        public IQueryItemContainer FuzzyQuery(string field, string value, int? prefixLength, int? maxExpansions, bool? transpositions)
        {
            FuzzyQueryItem query = new FuzzyQueryItem(field, value.ConvertTurkishCharacter(), prefixLength, maxExpansions, transpositions);
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Match sorgusu ile aynı yapıda çalışır. Kelime araması yapar. op değeri aranan kelimelerin AND yada OR olarak bağlanacağını ifade eder.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="op"></param>
        /// <param name="MinimumShouldMatchFixed"></param>
        /// <param name="MinimumShouldMatchPercentage"></param>
        /// <param name="fieldDescriptor"></param>
        /// <returns></returns>
        public IQueryItemContainer MultiMatchQuery(object value, EnumMultiQueryOperation? op = null, string MinimumShouldMatchFixed = null, string MinimumShouldMatchPercentage = null, params Expression<Func<T, object>>[] fieldDescriptors)
        {
            List<string> properties = new List<string>();
            foreach (var fieldDescriptor in fieldDescriptors)
            {
                properties.Add(fieldDescriptor.GetPropertyName());
            }

            MultiMatchQueryItem query = new MultiMatchQueryItem(properties.ToArray(), value, MinimumShouldMatchPercentage, MinimumShouldMatchFixed, op);
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçilen field değeri için arama yapılır.Cümle araması yapar. op değeri aranan kelimelerin AND yada OR olarak bağlanacağını ifade eder. Aranan ifade analiz edilir.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="op"></param>
        /// <param name="MinimumShouldMatchFixed"></param>
        /// <param name="MinimumShouldMatchPercentage"></param>
        /// <param name="fieldDescriptor"></param>
        /// <returns></returns>
        public IQueryItemContainer QueryStringQuery(string value, EnumMultiQueryOperation? op = null, string MinimumShouldMatchFixed = null, string MinimumShouldMatchPercentage = null, params Expression<Func<T, object>>[] fieldDescriptors)
        {
            List<string> properties = new List<string>();
            foreach (var fieldDescriptor in fieldDescriptors)
            {
                properties.Add(fieldDescriptor.GetPropertyName());
            }

            QueryStringQueryItem query = new QueryStringQueryItem(properties.ToArray(), value.ConvertTurkishCharacter(), MinimumShouldMatchPercentage, MinimumShouldMatchFixed, op);
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçilen field değeri için arama yapılır.Cümle araması yapar. op değeri aranan kelimelerin AND yada OR olarak bağlanacağını ifade eder. Aranan ifade analiz edilir.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="op"></param>
        /// <param name="MinimumShouldMatchFixed"></param>
        /// <param name="MinimumShouldMatchPercentage"></param>
        /// <param name="fieldDescriptor"></param>
        /// <returns></returns>
        public IQueryItemContainer QueryStringQuery(string value, EnumMultiQueryOperation? op = null, string MinimumShouldMatchFixed = null, string MinimumShouldMatchPercentage = null, params string[] fieldDescriptors)
        {
            QueryStringQueryItem query = new QueryStringQueryItem(fieldDescriptors, value.ConvertTurkishCharacter(), MinimumShouldMatchPercentage, MinimumShouldMatchFixed, op);
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Verilen field değeri için verilen değerler arasında arama yapılır. Numeric alanlarda kullanılması zorunludur.
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <param name="lessThan"></param>
        /// <param name="greaterThan"></param>
        /// <returns></returns>
        public IQueryItemContainer RangeQuery(Expression<Func<T, object>> fieldDescriptor, double? lessThan, double? greaterThan)
        {
            RangeQueryItem query = new RangeQueryItem(fieldDescriptor.GetPropertyName(), lessThan, greaterThan);
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçilen field için text araması yapar. Kelime araması yapar.
        /// * karakteri herhangi bir karakter serisi olarak kabul edilir. Boşluklarıda karakter olarak görür. 
        /// ? karakteri ise herhangi bir karakteri ifade eder. 
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer WildcardQuery(Expression<Func<T, object>> fieldDescriptor, string value)
        {
            WildcardQueryItem query = new WildcardQueryItem(fieldDescriptor.GetPropertyName(), value.ConvertTurkishCharacter());
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Verilen field adı için text araması yapar.
        /// * karakteri herhangi bir karakter serisi olarak kabul edilir. 
        /// ? karakteri ise herhangi bir karakteri ifade eder. 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer WildcardQuery(string field, string value)
        {
            WildcardQueryItem query = new WildcardQueryItem(field, value.ConvertTurkishCharacter());
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçilen field için regular expression araması yapar.
        /// </summary>
        /// <param name="fieldDescriptor"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer RegexpQuery(Expression<Func<T, object>> fieldDescriptor, string value)
        {
            RegexpQueryItem query = new RegexpQueryItem(fieldDescriptor.GetPropertyName(), value.ConvertTurkishCharacter());
            return new BasicQueryItemContainer(query);
        }

        /// <summary>
        /// Seçilen field için regular expression araması yapar.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IQueryItemContainer RegexpQuery(string field, string value)
        {
            RegexpQueryItem query = new RegexpQueryItem(field, value.ConvertTurkishCharacter());
            return new BasicQueryItemContainer(query);
        }

        public IQueryItemContainer Bool(params Func<BoolQueryBuilder<T>, QueryItemContainer>[] queries)
        {
            BoolQueryBuilder<T> boolQueryBuilder = new BoolQueryBuilder<T>();
            ComplexQueryItemContainer complexContainer = new ComplexQueryItemContainer();

            foreach (var item in queries)
            {
                QueryItemContainer container = item(boolQueryBuilder);
                if (container is ShouldQueryItemContainer)
                {
                    complexContainer.shouldQueryContainer = (ShouldQueryItemContainer)container;
                }
                else if (container is MustQueryItemContainer)
                {
                    complexContainer.mustQueryContainer = (MustQueryItemContainer)container;
                }
                else
                {
                    complexContainer.mustNotQueryContainer = (MustNotQueryItemContainer)container;
                }
            }

            return complexContainer;
        }
    }
}
