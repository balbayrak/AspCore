using AspCore.Business.Task.Abstract;
using System;

namespace AspCore.Business.Task.Concrete
{
    public abstract class TaskBuilder : ITaskBuilder
    {
        protected readonly IServiceProvider ServiceProvider;
        public TaskBuilder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        public abstract ITask GenerateTask<TEntity, TResult>(TaskEntity<TEntity> entity) where TEntity : class, new();

    }
}
