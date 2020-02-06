using AspCore.Business.Task.Concrete;
using AspCore.Dependency.Abstract;

namespace AspCore.Business.Task.Abstract
{
    public interface ITaskBuilder : ITransientType
    {
        ITask GenerateTask<TEntity, TResult>(TaskEntity<TEntity> entity) where TEntity : class, new();
    }
}
