using AspCore.Business.Abstract;
using AspCore.DataAccess.Abstract;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspCore.Business.Specifications.Abstract;

namespace AspCore.Business.Concrete
{
    public abstract class BaseEntityManager<TDataAccess, TEntity> : IEntityService<TEntity>
      where TDataAccess : IEntityRepository<TEntity>
      where TEntity : class, IEntity, new()
    {
        protected IServiceProvider ServiceProvider { get; }
        protected readonly TDataAccess DataAccess;
        protected ITransactionBuilder TransactionBuilder;
        public BaseEntityManager(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            DataAccess = ServiceProvider.GetRequiredService<TDataAccess>();
            TransactionBuilder = ServiceProvider.GetRequiredService<ITransactionBuilder>();
        }

        public virtual ServiceResult<bool> Add(params TEntity[] entities)
        {
            if (entities.Length > 1)
                return DataAccess.AddWithTransaction(entities);
            else
                return DataAccess.Add(entities);
        }

        public virtual ServiceResult<bool> Update(params TEntity[] entities)
        {
            if (entities.Length > 1)
                return DataAccess.UpdateWithTransaction(entities);
            else
                return DataAccess.Update(entities);
        }

        public virtual ServiceResult<bool> Delete(params Guid[] entityIds)
        {
            if (entityIds.Length > 1)
                return DataAccess.DeleteWithTransaction(entityIds);
            else
                return DataAccess.Delete(entityIds);
        }

        public virtual Task<ServiceResult<bool>> AddAsync(params TEntity[] entities)
        {
            if (entities.Length > 1)
                return DataAccess.AddWithTransactionAsync(entities);
            else
                return DataAccess.AddAsync(entities);
        }

        public virtual Task<ServiceResult<bool>> UpdateAsync(params TEntity[] entities)
        {
            if (entities.Length > 1)
                return DataAccess.UpdateWithTransactionAsync(entities);
            else
                return DataAccess.UpdateAsync(entities);
        }

        public virtual Task<ServiceResult<bool>> DeleteAsync(params Guid[] entityIds)
        {
            if (entityIds.Length > 1)
                return DataAccess.DeleteWithTransactionAsync(entityIds);
            else
                return DataAccess.DeleteAsync(entityIds);
        }

        public Task<ServiceResult<TEntity>> GetByIdAsync(EntityFilter<TEntity> setting)
        {
            return DataAccess.GetByIdAsync(setting.id);
        }

        public ServiceResult<TEntity> GetById(EntityFilter<TEntity> setting)
        {
            return DataAccess.GetById(setting.id);
        }

        public ServiceResult<IList<TEntity>> GetAll(EntityFilter<TEntity> setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            Expression<Func<TEntity, bool>> expression = null;
            List<SearchInfo> searchInfos = setting.GetSearchInfo();

            if (searchInfos != null && !string.IsNullOrEmpty(setting.search.searchValue))
            {
                expression = ExpressionBuilder.GetSearchExpression<TEntity>(searchInfos, setting.search.searchValue);
            }

            if (setting.sorters != null)
            {
                List<SortingExpression<TEntity>> sorters = null;
                if (setting.sorters != null)
                {
                    sorters = setting.sorters.ToSortingExpressionList();
                }
                return DataAccess.FindList(expression, sorters, setting.page, setting.pageSize);
            }
            else
            {
                return DataAccess.GetList(expression, setting.page, setting.pageSize);
            }
        }

        public Task<ServiceResult<IList<TEntity>>> GetAllAsync(EntityFilter<TEntity> setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            Expression<Func<TEntity, bool>> expression = null;
            List<SearchInfo> searchInfos = setting.GetSearchInfo();

            if (searchInfos != null && !string.IsNullOrEmpty(setting.search.searchValue))
            {
                expression = ExpressionBuilder.GetSearchExpression<TEntity>(searchInfos, setting.search.searchValue);
            }

            if (setting.sorters != null)
            {
                List<SortingExpression<TEntity>> sorters = null;
                if (setting.sorters != null)
                {
                    sorters = setting.sorters.ToSortingExpressionList();
                }
                return DataAccess.FindListAsync(expression, sorters, setting.page, setting.pageSize);
            }
            else
            {
                return DataAccess.GetListAsync(expression, setting.page, setting.pageSize);
            }
        }

        public Task<ServiceResult<List<TEntity>>> GetHistoriesByIdAsync(EntityFilter<TEntity> setting)
        {
            return DataAccess.GetHistoriesById(setting.id, setting.page, setting.pageSize);
        }

        public Task<ServiceResult<IList<TEntity>>> GetAllAsync(ISpecification<TEntity> specification)
        {
            return DataAccess.GetListAsync(specification.ToExpression());
        }
    }
}
