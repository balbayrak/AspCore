using AspCore.Business.General;
using AspCore.DataAccess.Abstract;
using AspCore.Dependency.Abstract;
using AspCore.Entities.EntityType;

namespace AspCore.Business.Task.Abstract
{
    public interface ITaskBuilder : ITransientType
    {
        TTask GenerateTask<TTask, TInput>(TInput input) where TTask : class, ITask, new();

        TTask GenerateEntityTask<TTask, TEntity>(TEntity entity, EnumCrudOperation? enumCrudOperation = null)
            where TTask : class, ITask
             where TEntity : class, IEntity;
    }
}
