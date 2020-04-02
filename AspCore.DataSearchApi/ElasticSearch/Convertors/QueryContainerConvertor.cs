using AspCore.ElasticSearchApiClient.General;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.ElasticSearchApiClient.QueryContiner.Abstract;
using AspCore.ElasticSearchApiClient.QueryItems;
using Nest;
using System;

namespace AspCore.DataSearchApi.ElasticSearch.Convertors
{
    public static class QueryContainerConvertor
    {
        public static QueryContainer GetQueryContainer<T>(this TermQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            if (!string.IsNullOrEmpty(queryItem.FieldName))
            {
                container = Query<T>.Term(Infer.Field(queryItem.FieldName), queryItem.Value);
            }
            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this TermsQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            if (!string.IsNullOrEmpty(queryItem.FieldName))
            {
                container = Query<T>.Terms(t => t.Field(Infer.Field(queryItem.FieldName)).Terms(queryItem.values));
            }
            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this PrefixQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            if (!string.IsNullOrEmpty(queryItem.FieldName))
            {
                container = Query<T>.Prefix(Infer.Field(queryItem.FieldName), queryItem.Value);
            }
            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this MatchQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            if (!string.IsNullOrEmpty(queryItem.FieldName))
            {
                container = Query<T>.Match(t => t.Field(Infer.Field(queryItem.FieldName)).Query(queryItem.Value));
            }

            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this MatchAllQueryItem queryItem) where T : class
        {
            QueryContainer container = Query<T>.MatchAll();
            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this MatchPhraseQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            if (!string.IsNullOrEmpty(queryItem.FieldName))
            {
                container = Query<T>.MatchPhrase(t => t.Field(Infer.Field(queryItem.FieldName)).Query(queryItem.Value));
            }

            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this MatchPhrasePrefixQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            if (!string.IsNullOrEmpty(queryItem.FieldName))
            {
                container = Query<T>.MatchPhrasePrefix(t => t.Field(Infer.Field(queryItem.FieldName)).Query(queryItem.Value));
            }

            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this MultiMatchQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            MinimumShouldMatch mns = null;
            if (!string.IsNullOrEmpty(queryItem.MinimumShouldMatchFixed) || !string.IsNullOrEmpty(queryItem.MinimumShouldMatchPercentage))
                mns = queryItem.MinimumShouldMatchFixed != null ? MinimumShouldMatch.Fixed(Convert.ToInt32(queryItem.MinimumShouldMatchFixed)) : MinimumShouldMatch.Percentage(Convert.ToDouble(queryItem.MinimumShouldMatchPercentage));


            Operator? op = null;
            if (queryItem.operation.HasValue)
            {
                op = (Operator)queryItem.operation.Value.GetHashCode();
            }
            if (mns != null)
                container = Query<T>.MultiMatch(t => t.Fields(k => k.Fields(queryItem.fields)).Operator(op).MinimumShouldMatch(mns));
            else
                container = Query<T>.MultiMatch(t => t.Fields(k => k.Fields(queryItem.fields)).Operator(op));

            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this DateRangeQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            Func<DateRangeQueryDescriptor<T>, IDateRangeQuery> selector = null;
            if (!string.IsNullOrEmpty(queryItem.LessThan.ToString()) || !string.IsNullOrEmpty(queryItem.GreaterThan.ToString()))
            {
                if (!queryItem.GreaterThan.HasValue)
                {
                    if (!string.IsNullOrEmpty(queryItem.FieldName))
                    {
                        selector = t => t.Field(Infer.Field(queryItem.FieldName)).LessThan(queryItem.LessThan.Value);
                    }
                }
                else if (!queryItem.LessThan.HasValue)
                {
                    if (!string.IsNullOrEmpty(queryItem.FieldName))
                    {
                        selector = t => t.Field(Infer.Field(queryItem.FieldName)).GreaterThan(queryItem.GreaterThan.Value);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(queryItem.FieldName))
                    {
                        selector = t => t.Field(Infer.Field(queryItem.FieldName)).GreaterThan(queryItem.GreaterThan.Value).LessThan(queryItem.LessThan.Value);
                    }
                }
            }
            if (selector != null)
                container = Query<T>.DateRange(selector);

            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this ExistQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            if (!string.IsNullOrEmpty(queryItem.FieldName))
            {
                container = Query<T>.Exists(t => t.Field(Infer.Field(queryItem.FieldName)));
            }
            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this FuzzyQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            if (!string.IsNullOrEmpty(queryItem.FieldName))
            {
                container = Query<T>.Fuzzy(t => t.Field(Infer.Field(queryItem.FieldName)).Value(queryItem.Value).PrefixLength(queryItem.PrefixLength).Transpositions(queryItem.Transpositions).MaxExpansions(queryItem.Max_Expansions));
            }
            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this QueryStringQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            MinimumShouldMatch mns = null;
            if (!string.IsNullOrEmpty(queryItem.MinimumShouldMatchFixed) || !string.IsNullOrEmpty(queryItem.MinimumShouldMatchPercentage))
                mns = queryItem.MinimumShouldMatchFixed != null ? MinimumShouldMatch.Fixed(Convert.ToInt32(queryItem.MinimumShouldMatchFixed)) : MinimumShouldMatch.Percentage(Convert.ToDouble(queryItem.MinimumShouldMatchPercentage));


            Operator? op = null;
            if (queryItem.operation.HasValue)
            {
                op = (Operator)queryItem.operation.Value.GetHashCode();
            }
            if (mns != null)
                container = Query<T>.QueryString(t => t.Fields(k => k.Fields(queryItem.fields)).DefaultOperator(op).MinimumShouldMatch(mns));
            else
                container = Query<T>.QueryString(t => t.Fields(k => k.Fields(queryItem.fields)).DefaultOperator(op));

            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this RangeQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            Func<NumericRangeQueryDescriptor<T>, INumericRangeQuery> selector = null;
            if (queryItem.LessThan.HasValue || queryItem.GreaterThan.HasValue)
            {
                if (!queryItem.GreaterThan.HasValue)
                {
                    if (!string.IsNullOrEmpty(queryItem.FieldName))
                    {
                        selector = t => t.Field(Infer.Field(queryItem.FieldName)).LessThan(queryItem.LessThan.Value);
                    }
                }
                else if (!queryItem.LessThan.HasValue)
                {
                    if (!string.IsNullOrEmpty(queryItem.FieldName))
                    {
                        selector = t => t.Field(queryItem.FieldName).GreaterThan(queryItem.GreaterThan.Value);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(queryItem.FieldName))
                    {
                        selector = t => t.Field(queryItem.FieldName).GreaterThan(queryItem.GreaterThan.Value).LessThan(queryItem.LessThan.Value);
                    }
                }
            }
            if (selector != null)
                container = Query<T>.Range(selector);

            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this WildcardQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            if (!string.IsNullOrEmpty(queryItem.FieldName))
            {
                container = Query<T>.Wildcard(t => t.Field(Infer.Field(queryItem.FieldName)).Value(queryItem.Value));
            }
            return container;
        }

        public static QueryContainer GetQueryContainer<T>(this RegexpQueryItem queryItem) where T : class
        {
            QueryContainer container = null;

            if (!string.IsNullOrEmpty(queryItem.FieldName))
            {
                container = Query<T>.Regexp(t => t.Field(Infer.Field(queryItem.FieldName)).Value(queryItem.Value));
            }
            return container;
        }

        public static QueryContainer GetInnerContainer<T>(this IComplexQueryItemContainer queryContainer) where T : class
        {
            QueryBuilder<T> queryBuilder = new QueryBuilder<T>();

            QueryContainer mustContainer = null;
            QueryContainer shouldContainer = null;
            QueryContainer mustNotContainer = null;
            QueryContainer filterContainer = null;

            if (queryContainer.mustQueryContainer != null)
            {
                mustContainer = new QueryContainer();

                if (queryContainer.mustQueryContainer.container.queries != null)
                {
                    foreach (var item in queryContainer.mustQueryContainer.container.queries)
                    {
                        mustContainer &= item.query.GetQueryContainer<T>();
                    }
                }

                if (!queryContainer.mustQueryContainer.container.complexQuery.CheckNull())
                {
                    QueryContainer complexMust = GetInnerContainer<T>(queryContainer.mustQueryContainer.container.complexQuery);
                    mustContainer &= complexMust;
                }
            }

            if (queryContainer.mustNotQueryContainer != null)
            {
                mustNotContainer = new QueryContainer();

                if (queryContainer.mustNotQueryContainer.container.queries != null)
                {
                    foreach (var item in queryContainer.mustNotQueryContainer.container.queries)
                    {
                        mustNotContainer &= (item.query.GetQueryContainer<T>());
                    }
                }

                if (!queryContainer.mustNotQueryContainer.container.complexQuery.CheckNull())
                {
                    QueryContainer complexMustNot = GetInnerContainer<T>(queryContainer.mustNotQueryContainer.container.complexQuery);
                    mustNotContainer &= complexMustNot;
                }
            }

            if (queryContainer.shouldQueryContainer != null)
            {
                shouldContainer = new QueryContainer();

                if (queryContainer.shouldQueryContainer.container.queries != null)
                {
                    foreach (var item in queryContainer.shouldQueryContainer.container.queries)
                    {
                        shouldContainer |= item.query.GetQueryContainer<T>();
                    }
                }


                if (!queryContainer.shouldQueryContainer.container.complexQuery.CheckNull())
                {
                    QueryContainer complexShould = GetInnerContainer<T>(queryContainer.shouldQueryContainer.container.complexQuery);
                    shouldContainer |= complexShould;
                }
            }

            if (queryContainer.filterQueryContainer != null)
            {
                filterContainer = new QueryContainer();

                if (queryContainer.filterQueryContainer.container.queries != null)
                {
                    foreach (var item in queryContainer.filterQueryContainer.container.queries)
                    {
                        filterContainer &= item.query.GetQueryContainer<T>();
                    }
                }


                if (!queryContainer.filterQueryContainer.container.complexQuery.CheckNull())
                {
                    QueryContainer complexFilter = GetInnerContainer<T>(queryContainer.filterQueryContainer.container.complexQuery);
                    filterContainer &= complexFilter;
                }
            }

            QueryContainer mainQuery = Query<T>.Bool(t => t.Must(mustContainer).Should(shouldContainer).MustNot(mustNotContainer).Filter(filterContainer));
            return mainQuery;
        }
    }
}
