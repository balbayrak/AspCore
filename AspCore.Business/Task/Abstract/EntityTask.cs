using AspCore.Business.Task.Concrete;
using AspCore.DataAccess.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.Business.Task.Abstract
{
    public abstract class EntityTask<TEntity, TDAL> : BaseTask<TEntity>
        where TEntity : class, IEntity, new()
        where TDAL : IEntityRepository<TEntity>
    {
        public abstract bool RunWithTransaction { get; }
        protected readonly TDAL _dataLayer;
        public EntityTask(TaskEntity<TEntity> taskEntity) : base(taskEntity)
        {
            _dataLayer = DependencyResolver.Current.GetService<TDAL>();
        }

        internal virtual ServiceResult<TResult> Create<TResult>()
        {
            if (RunWithTransaction)
                _transactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = _dataLayer.Add(taskEntity.entity);
                if (resultDAL.IsSucceeded)
                {
                    result.IsSucceeded = true;
                }

            }
            catch
            {
                if (RunWithTransaction)
                    _transactionBuilder.RollbackTransaction();
            }
            finally
            {
                if (RunWithTransaction)
                    _transactionBuilder.DisposeTransaction();
            }

            return result;
        }

        internal virtual ServiceResult<TResult> Update<TResult>()
        {
            if (RunWithTransaction)
                _transactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = _dataLayer.Update(taskEntity.entity);
                if (resultDAL.IsSucceeded)
                {
                    result.IsSucceeded = true;
                }

            }
            catch
            {
                if (RunWithTransaction)
                    _transactionBuilder.RollbackTransaction();
            }
            finally
            {
                if (RunWithTransaction)
                    _transactionBuilder.DisposeTransaction();
            }

            return result;
        }

        internal virtual ServiceResult<TResult> Delete<TResult>()
        {
            if (RunWithTransaction)
                _transactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = _dataLayer.Delete(taskEntity.entity);
                if (resultDAL.IsSucceeded)
                {
                    result.IsSucceeded = true;
                }

            }
            catch
            {
                if (RunWithTransaction)
                    _transactionBuilder.RollbackTransaction();
            }
            finally
            {
                if (RunWithTransaction)
                    _transactionBuilder.DisposeTransaction();
            }

            return result;
        }
    }
}
