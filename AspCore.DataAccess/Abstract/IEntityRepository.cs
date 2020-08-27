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
        Task<ServiceResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter);
        ServiceResult<TEntity> Get(Expression<Func<TEntity, bool>> filter);
        ServiceResult<IList<TEntity>> GetList(Expression<Func<TEntity, bool>> filter, int? page, int? pageSize);
        Task<ServiceResult<IList<TEntity>>> GetListAsync(Expression<Func<TEntity, bool>> filter, int? page, int? pageSize);
        Task<ServiceResult<IList<TEntity>>> GetListAsync(Expression<Func<TEntity, bool>> filter, int? page, int? pageSize, params Expression<Func<TEntity, object>>[] propertySelectors);

        Task<ServiceResult<IList<TEntity>>> GetListAsync(Expression<Func<TEntity, bool>> filter);
        Task<ServiceResult<IList<TEntity>>> GetListAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] propertySelectors);
        ServiceResult<TEntity[]> GetListWithIgnoreGlobalFilter();
        Task<ServiceResult<TEntity[]>> GetListWithIgnoreGlobalFilterAsync();

        ServiceResult<bool> Add(params TEntity[] entities);

        Task<ServiceResult<bool>> AddAsync(params TEntity[] entities);

        Task<ServiceResult<bool>> AddWithTransactionAsync(params TEntity[] entities);

        ServiceResult<bool> AddWithTransaction(params TEntity[] entities);

        ServiceResult<bool> Update(params TEntity[] entities);

        Task<ServiceResult<bool>> UpdateAsync(params TEntity[] entities);

        ServiceResult<bool> UpdateWithTransaction(params TEntity[] entities);

        Task<ServiceResult<bool>> UpdateWithTransactionAsync(params TEntity[] entities);

        ServiceResult<bool> Delete(params TEntity[] entities);

        Task<ServiceResult<bool>> DeleteAsync(params TEntity[] entities);

        ServiceResult<bool> Delete(params Guid[] entityIds);

        Task<ServiceResult<bool>> DeleteAsync(params Guid[] entityIds);

        Task<ServiceResult<bool>> DeleteWithTransactionAsync(params Guid[] entityIds);

        ServiceResult<bool> DeleteWithTransaction(params Guid[] entityIds);

        ServiceResult<TEntity> Find(Expression<Func<TEntity, bool>> filter);

        Task<ServiceResult<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter);

        Task<ServiceResult<TEntity>> GetByIdAsync(Guid id);

        ServiceResult<TEntity> GetById(Guid Id);

        ServiceResult<List<TEntity>> GetByIdList(params Guid[] entityIds);

        Task<ServiceResult<List<TEntity>>> GetByIdListAsync(params Guid[] entityIds);

        ServiceResult<IList<TEntity>> FindList(Expression<Func<TEntity, bool>> filter, List<SortingExpression<TEntity>> sorters = null, int? page = null, int? pageSize = null);

        Task<ServiceResult<IList<TEntity>>> FindListAsync(Expression<Func<TEntity, bool>> filter, List<SortingExpression<TEntity>> sorters = null, int? page = null, int? pageSize = null);


        Task<ServiceResult<List<TEntity>>> GetHistoriesById(Guid id, int? page = null, int? pageSize = null);
    }
}
