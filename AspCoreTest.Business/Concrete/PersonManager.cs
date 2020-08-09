using AspCore.Business.Concrete;
using AspCore.Business.Task.Abstract;
using AspCore.Entities.General;
using AspCoreTest.Business.Concrete.Tasks;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.DataSearch.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.Entities.SearchableEntities;
using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.Business.General;
using testbusiness.Abstract;
using System.Threading.Tasks;

namespace testbusiness.Concrete
{
    public class PersonManager : BaseComplexSearchableEntityManager<IPersonDal, Person, PersonSearchEntity, IPersonDataSearchEngine>, IPersonService
    {
        public PersonManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
        public override async Task<ServiceResult<PersonSearchEntity>> GetComplexEntity(Person entity)
        {
            return new ServiceResult<PersonSearchEntity>
            {
                IsSucceeded = true,
                Result = Mapper.MapProperties<Person, PersonSearchEntity>(entity)
            };
        }

        public override ServiceResult<bool> Add(params Person[] entities)
        {
            
            ITask personTask = new PersonTask(ServiceProvider, entities[0], EnumCrudOperation.CreateOperation);
            
            ITask personTask1 = new PersonTask(ServiceProvider, entities[0],EnumCrudOperation.CreateOperation);
            var res1 = personTask.Run().Result;

            var taskBuilder = ServiceProvider.GetRequiredService<ITaskFlowBuilder>();
            taskBuilder.AddTask(personTask);
            taskBuilder.AddTask(personTask1);

            var res = taskBuilder.RunTasks().Result;
            return (ServiceResult<bool>)res1;
        }
    }
}
