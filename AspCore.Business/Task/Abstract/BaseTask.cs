using AspCore.Business.Task.Concrete;
using AspCore.DataAccess.Abstract;
using AspCore.Dependency.Concrete;

namespace AspCore.Business.Task.Abstract
{
    public abstract class BaseTask<TEntity> : CoreTask
        where TEntity : class, new()
    {
        protected TaskEntity<TEntity> taskEntity { get; private set; }
        protected readonly ITransactionBuilder _transactionBuilder;
        public BaseTask(TaskEntity<TEntity> taskEntity) : base(taskEntity)
        {
            this.taskEntity = taskEntity;
            _transactionBuilder = DependencyResolver.Current.GetService<ITransactionBuilder>();
        }
    }
}
