using AspCore.Business.Task.Concrete;
using AspCore.DataAccess.Abstract;
using AspCore.DataSearch.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Business.Task.Abstract
{
    public abstract class SearchableEntityTask<TSearchableEntity, TDAL> : EntityTask<TSearchableEntity, TDAL>
        where TSearchableEntity : class, ISearchableEntity, new()
        where TDAL : IEntityRepository<TSearchableEntity>
    {
        private readonly IDataSearchEngine<TSearchableEntity> _dataSearchEngine;
        public SearchableEntityTask(IServiceProvider serviceProvider, TaskEntity<TSearchableEntity> TaskEntity) : base(serviceProvider,TaskEntity)
        {
            _dataSearchEngine = ServiceProvider.GetRequiredService<IDataSearchEngine<TSearchableEntity>>();
        }

        internal override ServiceResult<TResult> Create<TResult>()
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = DataLayer.Add(TaskEntity.entity);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = _dataSearchEngine.Create(TaskEntity.entity);
                    if (resultCache.IsSucceeded)
                    {
                        TransactionBuilder.CommitTransaction();
                        result.IsSucceeded = true;
                    }
                }
            }
            catch
            {
                TransactionBuilder.RollbackTransaction();
            }
            finally
            {
                TransactionBuilder.DisposeTransaction();
            }

            return result;
        }

        internal override ServiceResult<TResult> Update<TResult>()
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = DataLayer.Update(TaskEntity.entity);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = _dataSearchEngine.Update(TaskEntity.entity);
                    if (resultCache.IsSucceeded)
                    {
                        TransactionBuilder.CommitTransaction();
                        result.IsSucceeded = true;
                    }
                }
            }
            catch
            {
                TransactionBuilder.RollbackTransaction();
            }
            finally
            {
                TransactionBuilder.DisposeTransaction();
            }

            return result;
        }

        internal override ServiceResult<TResult> Delete<TResult>()
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = DataLayer.Delete(TaskEntity.entity);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = _dataSearchEngine.Delete(TaskEntity.entity);
                    if (resultCache.IsSucceeded)
                    {
                        TransactionBuilder.CommitTransaction();
                        result.IsSucceeded = true;
                    }
                }
            }
            catch
            {
                TransactionBuilder.RollbackTransaction();
            }
            finally
            {
                TransactionBuilder.DisposeTransaction();
            }

            return result;
        }
    }
}
