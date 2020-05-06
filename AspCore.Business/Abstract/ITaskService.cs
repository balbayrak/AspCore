using System.Collections.Generic;
using AspCore.Business.Task.Concrete;
using AspCore.Dependency.Abstract;
using AspCore.Entities.General;
using AspCore.Entities.User;

namespace AspCore.Business.Abstract
{
    public interface ITaskService<TActiveUser, TEntity> : ITransientType
        where TEntity : class, new()
        where TActiveUser : class, IAuthenticatedUser, new()
    {
        public TActiveUser activeUser { get; }

        ServiceResult<bool> RunAction(params TaskEntity<TEntity>[] taskEntities);

        ServiceResult<bool> RunAction(List<TaskEntity<TEntity>> taskEntities);


    }
}
