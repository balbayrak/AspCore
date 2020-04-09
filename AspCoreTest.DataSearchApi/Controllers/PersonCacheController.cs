using AspCore.BusinessApi;
using AspCore.DataSearchApi;
using AspCoreTest.Entities.Models;
using testbusiness.Abstract;

namespace AspCoreTest.DataSearchApi.Controllers
{
    public class PersonCacheController : BaseElasticSearchController<Person>
    {
    }
}