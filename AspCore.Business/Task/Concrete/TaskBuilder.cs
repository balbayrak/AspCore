using AspCore.Business.General;
using AspCore.Business.Task.Abstract;
using AspCore.DataAccess.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Business.Task.Concrete
{
    public class TaskBuilder : ITaskBuilder
    {
        private IServiceProvider _serviceProvider;


        public TaskBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public TTask GenerateTask<TTask, TInput>(TInput input)
            where TTask : class, ITask, new()
        {
            return (TTask)Activator.CreateInstance(typeof(TTask), input);
        }

        public TTask GenerateEntityTask<TTask, TEntity>(TEntity entity, EnumCrudOperation? enumCrudOperation = null)
            where TTask : class, ITask
             where TEntity : class, IEntity
        {
            return (TTask)Activator.CreateInstance(typeof(TTask), _serviceProvider, entity, enumCrudOperation);
        }

    }
}
