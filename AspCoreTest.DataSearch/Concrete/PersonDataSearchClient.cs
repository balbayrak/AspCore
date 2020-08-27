using AspCore.DataSearch.Concrete.ElasticSearch;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using AspCoreTest.DataSearch.Abstract;
using AspCoreTest.Entities.ModelFilters;
using AspCoreTest.Entities.SearchableEntities;
using System;
using System.Threading.Tasks;

namespace AspCoreTest.WebUI.DataSearch
{
    public class PersonDataSearchEngine : ESDataSearchEngine<PersonSearchEntity>, IPersonDataSearchEngine
    {
        public PersonDataSearchEngine(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public async Task<ServiceResult<DataSearchResult<PersonSearchEntity>>> FindByYetkiliKisi(PersonFilter filter)
        {
            ServiceResult<DataSearchResult<PersonSearchEntity>> result = null;

            int startIndex = 0;
            int takeCount = -1;
            if (filter.page.HasValue && filter.pageSize.HasValue)
            {
                startIndex = filter.page.Value * filter.pageSize.Value;
                takeCount = filter.pageSize.Value;
            }

            result = await ElasticClient.Read(t => t.Query(tt => tt.Bool(s => s.Filter
       (
           m => m.WildcardQuery(mm => mm.Name, "*" + filter.name.ToString() + "*"),
           m => m.TermQuery(mm => mm.IsDeleted, false)

       ))).From(startIndex).Size(takeCount)
       .TotalCountAgg(t => t.Id));




            return result;
        }
    }
}
