using AspCore.CacheEntityClient.General;
using AspCore.CacheEntityClient.QueryContiner.Abstract;
using AspCore.CacheEntityClient.QueryContiner.Concrete;
using System;
using System.Collections.Generic;

namespace AspCore.CacheEntityClient.QueryBuilder.Concrete
{
    public class BoolQueryBuilder<T> where T : class
    {
        internal QueryBuilder<T> queryBuilder;

        internal BoolQueryContainer container = null;
        public BoolQueryBuilder()
        {
            this.queryBuilder = new QueryBuilder<T>();
            this.container = new BoolQueryContainer();
        }

        public QueryItemContainer Should(params Func<QueryBuilder<T>, IQueryItemContainer>[] queries)
        {
            ShouldQueryItemContainer shouldContainer = new ShouldQueryItemContainer();
            foreach (var query in queries)
            {
                IQueryItemContainer item = query(queryBuilder);
                if (item is IComplexQueryItemContainer)
                {
                    ComplexQueryItemContainer complexItem = (ComplexQueryItemContainer)item;
                    shouldContainer.container = shouldContainer.container ?? new BoolQueryContainer();
                    shouldContainer.container.complexQuery.CheckAdd(complexItem);
                }
                else
                {
                    BasicQueryItemContainer basicItem = (BasicQueryItemContainer)item;
                    shouldContainer.container.queries = shouldContainer.container.queries ?? new List<BasicQueryItemContainer>();
                    shouldContainer.container.complexQuery = shouldContainer.container.complexQuery ?? new ComplexQueryItemContainer();
                    shouldContainer.container.queries.Add(basicItem);

                }
            }
            return shouldContainer;
        }

        public QueryItemContainer Must(params Func<QueryBuilder<T>, IQueryItemContainer>[] queries)
        {
            MustQueryItemContainer mustContainer = new MustQueryItemContainer();
            foreach (var query in queries)
            {
                IQueryItemContainer item = query(queryBuilder);
                if (item is IComplexQueryItemContainer)
                {
                    ComplexQueryItemContainer complexItem = (ComplexQueryItemContainer)item;
                    mustContainer.container = mustContainer.container ?? new BoolQueryContainer();
                    mustContainer.container.complexQuery = mustContainer.container.complexQuery ?? new ComplexQueryItemContainer();
                    mustContainer.container.complexQuery.CheckAdd(complexItem);
                }
                else
                {
                    BasicQueryItemContainer basicItem = (BasicQueryItemContainer)item;
                    mustContainer.container.queries = mustContainer.container.queries ?? new List<BasicQueryItemContainer>();
                    mustContainer.container.queries.Add(basicItem);
                }
            }
            return mustContainer;
        }

        public QueryItemContainer MustNot(params Func<QueryBuilder<T>, IQueryItemContainer>[] queries)
        {
            MustNotQueryItemContainer mustNotContainer = new MustNotQueryItemContainer();
            foreach (var query in queries)
            {
                IQueryItemContainer item = query(queryBuilder);
                if (item is IComplexQueryItemContainer)
                {
                    ComplexQueryItemContainer complexItem = (ComplexQueryItemContainer)item;
                    mustNotContainer.container = mustNotContainer.container ?? new BoolQueryContainer();
                    mustNotContainer.container.complexQuery = mustNotContainer.container.complexQuery ?? new ComplexQueryItemContainer();
                    mustNotContainer.container.complexQuery.CheckAdd(complexItem);
                }
                else
                {
                    BasicQueryItemContainer basicItem = (BasicQueryItemContainer)item;
                    mustNotContainer.container.queries = mustNotContainer.container.queries ?? new List<BasicQueryItemContainer>();
                    mustNotContainer.container.queries.Add(basicItem);
                }
            }
            return mustNotContainer;
        }

        public QueryItemContainer Filter(params Func<QueryBuilder<T>, IQueryItemContainer>[] queries)
        {
            FilterQueryItemContainer filterContainer = new FilterQueryItemContainer();
            foreach (var query in queries)
            {
                IQueryItemContainer item = query(queryBuilder);
                if (item is IComplexQueryItemContainer)
                {
                    ComplexQueryItemContainer complexItem = (ComplexQueryItemContainer)item;
                    filterContainer.container = filterContainer.container ?? new BoolQueryContainer();
                    filterContainer.container.complexQuery = filterContainer.container.complexQuery ?? new ComplexQueryItemContainer();
                    filterContainer.container.complexQuery.CheckAdd(complexItem);
                    
                }
                else
                {
                    BasicQueryItemContainer basicItem = (BasicQueryItemContainer)item;
                    filterContainer.container.queries = filterContainer.container.queries ?? new List<BasicQueryItemContainer>();
                    filterContainer.container.queries.Add(basicItem);
                }
            }
            return filterContainer;
        }

    }
}
