using System;
using System.Collections.Generic;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.Expression;

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
    }
}
