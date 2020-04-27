using AspCore.Business.Concrete;
using AspCore.Dependency.DependencyAttributes;
using AspCore.Entities.General;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.Entities.SearchableEntities;
using testbusiness.Abstract;

namespace testbusiness.Concrete
{
    [ExposedService(typeof(IPersonService))]
    public class PersonManager : BaseComplexSearchableEntityManager<IPersonDal, Person,PersonSearchEntity>, IPersonService
    {
        public PersonManager()
        {

        }
        public override ServiceResult<PersonSearchEntity> GetComplexEntity(Person entity)
        {
            return new ServiceResult<PersonSearchEntity>
            {
                IsSucceeded = true,
                Result = mapper.MapProperties<Person, PersonSearchEntity>(entity)
            };
        }
    }
}
