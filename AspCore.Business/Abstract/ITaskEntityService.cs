using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.Dependency.Abstract;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.User;

namespace AspCore.Business.Abstract
{
    public interface ITaskEntityService<TActiveUser, TEntity> : ITaskService<TActiveUser, TEntity>, IScopedType
        where TEntity : class, IEntity, new()
        where TActiveUser : class, IActiveUser, new()
    {
        ServiceResult<TEntity> GetById(EntityFilter<TEntity> setting);

        ServiceResult<IList<TEntity>> GetAll(EntityFilter<TEntity> setting);

        Task<ServiceResult<IList<TEntity>>> GetAllAsync(EntityFilter<TEntity> setting);

    }
}
