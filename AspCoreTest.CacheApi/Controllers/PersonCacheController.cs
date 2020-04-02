using AspCoreTest.Entities.Models;

namespace AspCoreTest.CacheApi.Controllers
{
    public class PersonCacheController : BaseElasticSearchController<ICacheEntityProvider<Person>, Person>
    {
    }
}