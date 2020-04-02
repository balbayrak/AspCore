using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using Nest;
using System.Collections.Generic;

namespace AspCore.DataSearchApi.ElasticSearch.Convertors
{
    public static class SortConvertor
    {
        public static List<ISort> GetSortItems<T>(this SortItem sortItem) where T : class
        {
            List<ISort> items = null;
            if (sortItem != null && ((sortItem.ascendingFields != null && sortItem.ascendingFields.Length > 0) || (sortItem.descendingFields != null && sortItem.descendingFields.Length > 0)))
            {
                items = new List<ISort>();

                if (sortItem.ascendingFields != null)
                {
                    foreach (var ascendingField in sortItem.ascendingFields)
                    {
                        items.Add(new FieldSort { Field = Infer.Field(ascendingField), Order = SortOrder.Ascending });
                    }
                }

                if (sortItem.descendingFields != null)
                {
                    foreach (var descendingField in sortItem.descendingFields)
                    {
                        items.Add(new FieldSort { Field = Infer.Field(descendingField), Order = SortOrder.Descending });
                    }
                }
            }
            return items;
        }
    }
}
