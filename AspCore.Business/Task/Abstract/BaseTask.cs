using AspCore.Business.Task.Concrete;
using AspCore.DataAccess.Abstract;
using AspCore.Dependency.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Business.Task.Abstract
{
    public abstract class BaseTask<TEntity> : CoreTask
        where TEntity : class, new()
    {
        protected TaskEntity<TEntity> TaskEntity { get; private set; }
        protected readonly IServiceProvider ServiceProvider;
        protected readonly ITransactionBuilder TransactionBuilder;
        public BaseTask(IServiceProvider serviceProvider, TaskEntity<TEntity> taskEntity) : base(taskEntity)
        {
            this.TaskEntity = taskEntity;
            ServiceProvider = serviceProvider;
            TransactionBuilder = ServiceProvider.GetRequiredService<ITransactionBuilder>();
        }
    }
}
