using AspCore.DataSearchApi;
using AspCoreTest.Entities.SearchableEntities;
using System;

namespace AspCoreTest.DataSearchApi.Controllers
{
    public class PersonCacheController : BaseElasticSearchController<PersonSearchEntity>
    {
        public PersonCacheController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}