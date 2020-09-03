using AspCore.Business.Concrete;
using AspCore.Business.General;
using AspCore.Business.Task.Abstract;
using AspCore.Entities.General;
using AspCoreTest.Business.Concrete.Tasks;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.DataSearch.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.Entities.SearchableEntities;
using Microsoft.Extensions.DependencyInjection;
using System;
using AspCoreTest.Dtos.Dtos;
using testbusiness.Abstract;
using System.Threading.Tasks;

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
        
    }
}
