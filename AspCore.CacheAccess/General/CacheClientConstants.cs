namespace AspCore.CacheAccess.General
{
    internal sealed class CacheClientConstants
    {
        public struct ELK_ConstString
        {
            public static readonly string KEYWORD_FIELD = ".keyword";
        }

        public struct CacheApiActions
        {
            public static readonly string CREATE_ACTION_NAME = "CreateCacheData";
            public static readonly string READ_ACTION_NAME = "ReadCacheData";
            public static readonly string UPDATE_ACTION_NAME = "UpdateCacheData";
            public static readonly string DELETE_ACTION_NAME = "DeleteCacheData";
            public static readonly string GETDATA_ACTION_NAME = "GetCacheData";
            public static readonly string MIN_MAX_ACTION_NAME = "MinMaxCacheData";
        }
    }
}
