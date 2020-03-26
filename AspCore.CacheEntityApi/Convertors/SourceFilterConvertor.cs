using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using Nest;

namespace AspCore.CacheEntityApi.Convertors
{
    public static class SourceFilterConvertor
    {
        public static SourceFilter GetSourceFilter(this FilterSource filterSource)
        {
            SourceFilter sf = null;

            if (filterSource.includeAll)
            {
                sf = SourceFilter.IncludeAll;
            }

            if (filterSource.excludeAll)
            {
                sf = SourceFilter.ExcludeAll;
            }

            if (filterSource.includeFields != null && filterSource.includeFields.Length > 0)
            {
                sf = new SourceFilter();
                sf.Includes = filterSource.includeFields;
            }

            if (filterSource.excludeFields != null && filterSource.excludeFields.Length > 0)
            {
                sf = new SourceFilter();
                sf.Excludes = filterSource.excludeFields;
            }
            return sf = sf ?? SourceFilter.IncludeAll;
        }
    }
}
