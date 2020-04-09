using AspCore.ElasticSearchApiClient.QueryContiner.Concrete;
using System.ComponentModel.DataAnnotations;

namespace AspCore.ElasticSearchApiClient.QueryBuilder.Concrete
{
    public class SearchRequestItem
    {
        public FilterSource sourceFilter;
        public SortItem sortItem;
        public string indexKey;
        public int size;
        public int from;

        public ComplexQueryItemContainer queryContainer;
        public ComplexQueryItemContainer postFilterQueryContainer;
        public string IdFieldPropertyName;

        public SearchRequestItem()
        {

        }
    }
}
