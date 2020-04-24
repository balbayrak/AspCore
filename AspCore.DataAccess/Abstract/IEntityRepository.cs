using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspCore.Dependency.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Utilities;

namespace AspCore.DataAccess.Abstract
{
    public interface IEntityRepository<TEntity> : IScopedType where TEntity : class, IEntity, new()
    {
        ServiceResult<TEntity> Get(Expression<Func<TEntity, bool>> filter);

        ServiceResult<IList<TEntity>> GetList(Expression<Func<TEntity, bool>> filter, int? page, int? pageSize);

        ServiceResult<TEntity[]> GetListWithIgnoreGlobalFilter();

        Task<ServiceResult<IList<TEntity>>> GetListAsync(Expression<Func<TEntity, bool>> filter, int? page, int? pageSize);

        ServiceResult<bool> Add(params TEntity[] entities);

        ServiceResult<bool> AddWithTransaction(params TEntity[] entities);

        ServiceResult<bool> Update(params TEntity[] entities);

        ServiceResult<bool> UpdateWithTransaction(params TEntity[] entities);

        ServiceResult<bool> Delete(params TEntity[] entities);

        ServiceResult<bool> Delete(params Guid[] entityIds);

        ServiceResult<bool> DeleteWithTransaction(params Guid[] entityIds);

        ServiceResult<TEntity> Find(Expression<Func<TEntity, bool>> filter);

        ServiceResult<TEntity> GetById(Guid Id);

        ServiceResult<List<TEntity>> GetByIdList(params Guid[] entityIds);

        ServiceResult<IList<TEntity>> FindList(Expression<Func<TEntity, bool>> filter, List<SortingExpression<TEntity>> sorters = null, int? page = null, int? pageSize = null);

        Task<ServiceResult<IList<TEntity>>> FindListAsync(Expression<Func<TEntity, bool>> filter, List<SortingExpression<TEntity>> sorters = null, int? page = null, int? pageSize = null);

        ServiceResult<bool> ProcessEntityWithState(params TEntity[] entities);

        ServiceResult<bool> ProcessEntityWithStateNotTransaction(TEntity item);

        ServiceResult<bool> ProcessEntitiesWithState(List<TEntity> items);

    }
}
