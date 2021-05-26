using AspCore.Business.Concrete;
using AspCore.Entities.General;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.DataSearch.Abstract;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;
using AspCoreTest.Entities.SearchableEntities;
using System;
using System.Threading.Tasks;
using AspCoreTest.Business.Interceptors;
using AspCoreTest.Business.Validators;
using testbusiness.Abstract;

namespace testbusiness.Concrete
{
    public class PersonManager : BaseComplexSearchableEntityManager<IPersonDal, Person, PersonDto, PersonSearchEntity, IPersonDataSearchEngine>, IPersonService
    {
        public PersonManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
        public override async Task<ServiceResult<PersonSearchEntity>> GetComplexEntity(Person entity)
        {
            return new ServiceResult<PersonSearchEntity>
            {
                IsSucceeded = true,
                Result = AutoObjectMapper.Mapper.Map<Person, PersonSearchEntity>(entity)
            };
        }
        public override ServiceResult<bool> Add(params PersonDto[] entities)
        {
           entities[0].Admin=new AdminDto();
           entities[0].Admin.Description = "eewrew";
             return  base.Add(entities);
        }
        public override Task<ServiceResult<bool>> AddAsync(params PersonDto[] entities)
        {
            entities[0].Admin = new AdminDto();
            entities[0].Admin.Description = "eewrew";
            return base.AddAsync(entities);
        }
        [ValidationAspect(typeof(PersonValidator))]
        public ServiceResult<bool> Add(PersonDto entities)
        {
           return AddAsync(entities).Result;
        }
    }
}
