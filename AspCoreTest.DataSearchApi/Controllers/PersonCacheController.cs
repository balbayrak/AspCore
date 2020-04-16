using AspCore.DataSearchApi;
using AspCoreTest.Entities.SearchableEntities;

namespace AspCoreTest.DataSearchApi.Controllers
{
    public class PersonCacheController : BaseElasticSearchController<PersonSearchEntity>
    {
    }
}