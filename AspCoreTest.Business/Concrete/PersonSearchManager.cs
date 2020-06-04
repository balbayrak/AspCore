using AspCore.Business.Concrete;
using AspCore.Entities.General;
using AspCoreTest.Business.Abstract;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.Entities.SearchableEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCoreTest.Business.Concrete
{
    public class PersonSearchManager : BaseSearchManager<IPersonDal,Person,PersonSearchEntity>, IPersonSearchEntityService
    {
        public PersonSearchManager(IServiceProvider serviceProvider) : base(serviceProvider)
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
