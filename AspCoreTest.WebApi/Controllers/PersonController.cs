using AspCore.WebApi;
using AspCoreTest.Entities.Models;
using testbusiness.Abstract;


namespace AspCoreTest.WebApi.Controllers
{

    public class PersonController : BaseEntityController<IPersonService, Person>
    {
      
    }
}