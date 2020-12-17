using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.Expression;
using AspCore.Utilities;
using AspCore.Utilities.ExpressionUtilities;

namespace AspCore.Extension
{
    public static class EntityFilterExt
    {
        public static List<SearchInfo> GetSearchInfo(this EntityFilter filter)
        {
            List<SearchInfo> searchInfo = null;

            if (filter.search != null && !string.IsNullOrEmpty(filter.search.searchableColumns))
            {
                searchInfo = new List<SearchInfo>();
                string[] properties = filter.search.searchableColumns.Split(CoreConstants.General.SPLIT_CHAR1);
                foreach (var item in properties)
                {
                    string[] propertyInfo = item.Split(CoreConstants.General.SPLIT_CHAR);
                    searchInfo.Add(new SearchInfo
                    {
                        operation = (Operation)Convert.ToInt32(propertyInfo[0]),
                        propertyName = propertyInfo[1]
                    });
                }
            }

            return searchInfo;
        }

        public static List<SearchInfo> GetSearchInfo(this SearchType search)
        {
            List<SearchInfo> searchInfo = null;

            if (search != null && !string.IsNullOrEmpty(search.searchableColumns))
            {
                searchInfo = new List<SearchInfo>();
                string[] properties = search.searchableColumns.Split(CoreConstants.General.SPLIT_CHAR1);
                foreach (var item in properties)
                {
                    string[] propertyInfo = item.Split(CoreConstants.General.SPLIT_CHAR);
                    searchInfo.Add(new SearchInfo
                    {
                        operation = (Operation)Convert.ToInt32(propertyInfo[0]),
                        propertyName = propertyInfo[1]
                    });
                }
            }

            return searchInfo;
        }

        public static Expression<Func<T, bool>> GetSearchExpression<T>(this EntityFilter filter)
        {
            List<SearchInfo> searchInfos = filter.GetSearchInfo();
            if (searchInfos != null && !string.IsNullOrEmpty(filter.search.searchValue))
            {
                return ExpressionBuilder.GetSearchExpression<T>(searchInfos, filter.search.searchValue);
            }
            return null;
        }

        public static Expression<Func<T, bool>> GetSearchExpression<T>(this SearchType search)
        {
            List<SearchInfo> searchInfos = search.GetSearchInfo();
            if (searchInfos != null && !string.IsNullOrEmpty(search.searchValue))
            {
                return ExpressionBuilder.GetSearchExpression<T>(searchInfos, search.searchValue);
            }
            return null;
        }
    }
}
