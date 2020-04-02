using AspCore.ElasticSearchApiClient.QueryBuilder.Abstract;
using AspCore.ElasticSearchApiClient.QueryItems;
using Nest;

namespace AspCore.DataSearchApi.ElasticSearch.Convertors
{
    public static class ElasticClientConvertor
    {
        public static QueryContainer GetQueryContainer<T>(this IQueryItem queryItem) where T : class
        {
            if (queryItem is TermQueryItem)
                return ((TermQueryItem)queryItem).GetQueryContainer<T>();
            if (queryItem is TermsQueryItem)
                return ((TermsQueryItem)queryItem).GetQueryContainer<T>();
            else if (queryItem is DateRangeQueryItem)
                return ((DateRangeQueryItem)queryItem).GetQueryContainer<T>();
            else if (queryItem is MatchPhrasePrefixQueryItem)
                return ((MatchPhrasePrefixQueryItem)queryItem).GetQueryContainer<T>();
            else if (queryItem is ExistQueryItem)
                return ((ExistQueryItem)queryItem).GetQueryContainer<T>();
            else if (queryItem is FuzzyQueryItem)
                return ((FuzzyQueryItem)queryItem).GetQueryContainer<T>();
            else if (queryItem is MatchPhraseQueryItem)
                return ((MatchPhraseQueryItem)queryItem).GetQueryContainer<T>();
            else if (queryItem is MatchQueryItem)
                return ((MatchQueryItem)queryItem).GetQueryContainer<T>();
            else if (queryItem is MultiMatchQueryItem)
                return ((MultiMatchQueryItem)queryItem).GetQueryContainer<T>();
            else if (queryItem is PrefixQueryItem)
                return ((PrefixQueryItem)queryItem).GetQueryContainer<T>();
            else if (queryItem is QueryStringQueryItem)
                return ((QueryStringQueryItem)queryItem).GetQueryContainer<T>();
            else if (queryItem is RangeQueryItem)
                return ((RangeQueryItem)queryItem).GetQueryContainer<T>();
            else if (queryItem is RegexpQueryItem)
                return ((RegexpQueryItem)queryItem).GetQueryContainer<T>();
            else if (queryItem is WildcardQueryItem)
                return ((WildcardQueryItem)queryItem).GetQueryContainer<T>();
            else return null;
        }
    }
}
