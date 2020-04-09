using AspCore.Business.Concrete;
using AspCore.Entities.General;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using testbusiness.Abstract;

namespace testbusiness.Concrete
{
    public class PersonManager : BaseComplexSearchableEntityManager<IPersonDal, Person, Person>, IPersonService
    {
        public override ServiceResult<Person> GetComplexEntity(Person entity)
        {
            return new ServiceResult<Person>
            {
                IsSucceeded = true,
                Result = entity
            };
        }
    }
}
