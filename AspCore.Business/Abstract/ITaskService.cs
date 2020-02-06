using System.Collections.Generic;
using AspCore.Business.Task.Concrete;
using AspCore.Dependency.Abstract;
using AspCore.Entities.General;
using AspCore.Entities.User;

namespace AspCore.Business.Abstract
{
    public interface ITaskService<TActiveUser, TEntity> : IScopedType
        where TEntity : class, new()
        where TActiveUser : class, IActiveUser, new()
    {
        public TActiveUser activeUser { get; }

        ServiceResult<bool> RunAction(params TaskEntity<TEntity>[] taskEntities);

        ServiceResult<bool> RunAction(List<TaskEntity<TEntity>> taskEntities);


    }
}
