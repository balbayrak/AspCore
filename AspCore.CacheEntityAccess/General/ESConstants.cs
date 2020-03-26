namespace AspCore.CacheEntityAccess.General
{
    public class ESConstants
    {
        public struct ErrorMessages
        {
            public const string ES_CREATE_INDEX_ERROR_OCCURRED = "ElasticSearch Create Index Failure!";
            public const string ES_DELETE_INDEX_ERROR_OCCURRED = "ElasticSearch Delete Index Failure!";
            public const string ES_BULK_INDEX_ERROR_OCCURRED = "ElasticSearch Bulk Index Failure!";
            public const string ES_CREATE_INDEX_ITEM_ERROR_OCCURRED = "ElasticSearch Create Index Item Failure!";
            public const string ES_UPDATE_INDEX_ITEM_ERROR_OCCURRED = "ElasticSearch Update Index Item Failure!";
            public const string ES_DELETE_INDEX_ITEM_ERROR_OCCURRED = "ElasticSearch Delete Index Item Failure!";
            public const string ES_SEARCH_INDEX_ITEM_ERROR_OCCURRED = "ElasticSearch Search Index Failure!";
        }

        public struct AGGREGATION_KEYS
        {
            public static readonly string VALUE_COUNT_AGG = "totalcount";
            public static readonly string VALUE_SEARCH_COUNT_AGG = "searchcount";
            public static readonly string VALUE_MIN_AGG = "min";
            public static readonly string VALUE_MAX_AGG = "max";
        }
    }
}
