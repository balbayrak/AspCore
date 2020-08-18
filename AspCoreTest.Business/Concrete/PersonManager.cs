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
            var entityMap = AutoObjectMapper.Mapper.Map<Person[]>(entities);

           // var res = TaskBuilder.GenerateEntityTask<PersonTask, Person>(entityMap[0], null).Run().Result;

            //ITask personTask = new PersonTask(ServiceProvider, entityMap[0], EnumCrudOperation.CreateOperation).AddValidator(new PersonTaskValidator(entityMap[0]));

            //ITask personTask1 = new PersonTask(ServiceProvider, entityMap[0], EnumCrudOperation.CreateOperation);
            //var res1 = personTask.Run().Result;

            var taskBuilder = ServiceProvider.GetRequiredService<ITaskFlowBuilder>();
            taskBuilder.AddTask(TaskBuilder.GenerateEntityTask<PersonTask, Person>(entityMap[0]));
            taskBuilder.AddTask(TaskBuilder.GenerateEntityTask<PersonTask, Person>(entityMap[0]));

            var res = taskBuilder.RunTasks().Result;
            return (ServiceResult<bool>)res;
        }

        public override async Task<ServiceResult<bool>> AddAsync(params PersonDto[] entities)
        {
            var entityMap = AutoObjectMapper.Mapper.Map<Person[]>(entities);

            //var res = await TaskBuilder.GenerateEntityTask<PersonTask, Person>(entityMap[0], null).Run();

           
            var taskBuilder = ServiceProvider.GetRequiredService<ITaskFlowBuilder>();
            taskBuilder.AddTask(TaskBuilder.GenerateEntityTask<PersonTask, Person>(entityMap[0]));
            taskBuilder.AddTask(TaskBuilder.GenerateEntityTask<PersonTask, Person>(entityMap[0]));

            var res = taskBuilder.RunTasks().Result;
            return (ServiceResult<bool>)res;
        }
    }
}
