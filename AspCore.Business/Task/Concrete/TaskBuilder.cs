using AspCore.Business.Task.Abstract;

namespace AspCore.Business.Task.Concrete
{
    public abstract class TaskBuilder : ITaskBuilder
    {
        public abstract ITask GenerateTask<TEntity, TResult>(TaskEntity<TEntity> entity) where TEntity : class, new();

    }
}
