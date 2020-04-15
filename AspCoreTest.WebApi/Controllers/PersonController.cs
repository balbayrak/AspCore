using AspCore.BusinessApi;
using AspCoreTest.Entities.Models;
using testbusiness.Abstract;

namespace AspCoreTest.WebApi.Controllers
{
    public class PersonController : BaseEntityController<Person, IPersonService>
    {
      
    }
}