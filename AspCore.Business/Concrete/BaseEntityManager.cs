using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspCore.Business.Abstract;
using AspCore.DataAccess.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.Utilities;

namespace AspCore.Business.Concrete
{
    public abstract class BaseEntityManager<TDataAccess, TEntity> : IEntityService<TEntity>
      where TDataAccess : IEntityRepository<TEntity>
      where TEntity : class, IEntity, new()
    {
        protected readonly TDataAccess _dataAccess;
        protected ITransactionBuilder _transactionBuilder;
        public BaseEntityManager()
        {
            _dataAccess = DependencyResolver.Current.GetService<TDataAccess>(); ;
            _transactionBuilder = DependencyResolver.Current.GetService<ITransactionBuilder>();
        }

        public virtual ServiceResult<bool> Add(params TEntity[] entities)
        {
            if (entities.Length > 1)
                return _dataAccess.AddWithTransaction(entities);
            else
                return _dataAccess.Add(entities);
        }

        public virtual ServiceResult<bool> Update(params TEntity[] entities)
        {
            if (entities.Length > 1)
                return _dataAccess.UpdateWithTransaction(entities);
            else
                return _dataAccess.Update(entities);
        }

        public virtual ServiceResult<bool> Delete(params Guid[] entityIds)
        {
            if (entityIds.Length > 1)
                return _dataAccess.DeleteWithTransaction(entityIds);
            else
                return _dataAccess.Delete(entityIds);
        }

        public ServiceResult<TEntity> GetById(EntityFilter<TEntity> setting)
        {
            return _dataAccess.GetById(setting.id);
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
                    sorters = setting.sorters.ToSortingExpressionList<TEntity>();
                }
                return _dataAccess.FindList(expression, sorters, setting.page, setting.pageSize);
            }
            else
            {
                return _dataAccess.GetList(expression, setting.page, setting.pageSize);
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

            if ( setting.sorters != null)
            {
                List<SortingExpression<TEntity>> sorters = null;
                if (setting.sorters != null)
                {
                    sorters = setting.sorters.ToSortingExpressionList<TEntity>();
                }
                return _dataAccess.FindListAsync(expression, sorters, setting.page, setting.pageSize);
            }
            else
            {
                return _dataAccess.GetListAsync(expression, setting.page, setting.pageSize);
            }
        }
    }
}
