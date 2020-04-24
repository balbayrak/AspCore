using AspCore.Business.Concrete;
using AspCore.Entities.General;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.Entities.SearchableEntities;
using System;
using testbusiness.Abstract;

namespace testbusiness.Concrete
{
    public class PersonManager : BaseComplexSearchableEntityManager<IPersonDal, Person, PersonSearchEntity>, IPersonService
    {
        public PersonManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
        public override ServiceResult<PersonSearchEntity> GetComplexEntity(Person entity)
        {
            return new ServiceResult<PersonSearchEntity>
            {
                IsSucceeded = true,
                Result = Mapper.MapProperties<Person, PersonSearchEntity>(entity)
            };
        }
    }
}
