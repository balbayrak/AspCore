using AspCore.Business.Task.Concrete;
using AspCore.DataAccess.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Business.Task.Abstract
{
    public abstract class EntityTask<TEntity, TDAL> : BaseTask<TEntity>
        where TEntity : class, IEntity, new()
        where TDAL : IEntityRepository<TEntity>
    {
        public abstract bool RunWithTransaction { get; }
        protected readonly TDAL DataLayer;
        public EntityTask(IServiceProvider serviceProvider, TaskEntity<TEntity> taskEntity) : base(serviceProvider, taskEntity)
        {
            DataLayer = ServiceProvider.GetRequiredService<TDAL>();
        }

        internal virtual ServiceResult<TResult> Create<TResult>()
        {
            if (RunWithTransaction)
                TransactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = DataLayer.Add(TaskEntity.entity);
                if (resultDAL.IsSucceeded)
                {
                    result.IsSucceeded = true;
                }

            }
            catch
            {
                if (RunWithTransaction)
                    TransactionBuilder.RollbackTransaction();
            }
            finally
            {
                if (RunWithTransaction)
                    TransactionBuilder.DisposeTransaction();
            }

            return result;
        }

        internal virtual ServiceResult<TResult> Update<TResult>()
        {
            if (RunWithTransaction)
                TransactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = DataLayer.Update(TaskEntity.entity);
                if (resultDAL.IsSucceeded)
                {
                    result.IsSucceeded = true;
                }

            }
            catch
            {
                if (RunWithTransaction)
                    TransactionBuilder.RollbackTransaction();
            }
            finally
            {
                if (RunWithTransaction)
                    TransactionBuilder.DisposeTransaction();
            }

            return result;
        }

        internal virtual ServiceResult<TResult> Delete<TResult>()
        {
            if (RunWithTransaction)
                TransactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = DataLayer.Delete(TaskEntity.entity);
                if (resultDAL.IsSucceeded)
                {
                    result.IsSucceeded = true;
                }

            }
            catch
            {
                if (RunWithTransaction)
                    TransactionBuilder.RollbackTransaction();
            }
            finally
            {
                if (RunWithTransaction)
                    TransactionBuilder.DisposeTransaction();
            }

            return result;
        }
    }
}
