using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.Business.Abstract;
using AspCore.Business.Task.Abstract;
using AspCore.Business.Task.Concrete;
using AspCore.DataAccess.Abstract;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Extension;
using AspCore.Utilities;

namespace AspCore.Business.Concrete
{
    public abstract class BaseTaskEntityManager<TActiveUser, TEntity, TDAL,TTaskBuilder> : BaseTaskManager<TActiveUser, TEntity, TTaskBuilder>, ITaskEntityService<TActiveUser, TEntity>
        where TDAL : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TActiveUser : class, IActiveUser, new()
        where TTaskBuilder : TaskBuilder, ITaskBuilder
    {
        private readonly TDAL _dataLayer;

        public BaseTaskEntityManager(TDAL dataLayer) : base()
        {
            _dataLayer = dataLayer;
        }

        public ServiceResult<TEntity> GetById(EntityFilter<TEntity> setting)
        {
            return _dataLayer.GetById(setting.id);
        }

        public ServiceResult<IList<TEntity>> GetAll(EntityFilter<TEntity> setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            if (setting.sorters != null)
            {

                List<SortingExpression<TEntity>> sorters = null;
                if (setting.sorters != null)
                {
                    sorters = setting.sorters.ToSortingExpressionList<TEntity>();
                }
                return _dataLayer.FindList(null, sorters, setting.page, setting.pageSize);
            }
            else
            {
                return _dataLayer.GetList(null, setting.page, setting.pageSize);
            }
        }

        public Task<ServiceResult<IList<TEntity>>> GetAllAsync(EntityFilter<TEntity> setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }
            if (setting.sorters != null)
            {
                List<SortingExpression<TEntity>> sorters = null;
                if (setting.sorters != null)
                {
                    sorters = setting.sorters.ToSortingExpressionList<TEntity>();
                }
                return _dataLayer.FindListAsync(null, sorters, setting.page, setting.pageSize);
            }
            else
            {
                return _dataLayer.GetListAsync(null, setting.page, setting.pageSize);
            }
        }

    }
}
