using AspCore.Business.General;
using AspCore.DataAccess.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AspCore.Business.Task.Abstract
{
    public abstract class EntityTask<TEntity, TResult, TDAL> : CoreTask<TEntity, TResult>
        where TEntity : class, IEntity, new()
        where TDAL : IEntityRepository<TEntity>
    {
        public abstract bool RunWithTransaction { get; }
        protected readonly TDAL DataLayer;
        protected readonly ITransactionBuilder TransactionBuilder;
        protected TEntity Entity { get; set; }
        protected EnumCrudOperation? CrudOperation { get; set; }

        protected IServiceProvider ServiceProvider { get; private set; }

        protected EntityTask(IServiceProvider serviceProvider, TEntity entity, EnumCrudOperation? enumCrudOperation = null) : base(entity)
        {
            ServiceProvider = serviceProvider;
            Entity = entity;
            CrudOperation = enumCrudOperation;

            DataLayer = ServiceProvider.GetRequiredService<TDAL>();
            TransactionBuilder = ServiceProvider.GetRequiredService<ITransactionBuilder>();
        }


        public override async Task<ServiceResult<TResult>> RunTask()
        {
            ServiceResult<TResult> serviceResult = new ServiceResult<TResult>();

            if (CrudOperation.HasValue)
            {
                if (CrudOperation.Value == EnumCrudOperation.CreateOperation)
                    serviceResult = await CreateAsync();
                else if (CrudOperation.Value == EnumCrudOperation.UpdateOperation)
                    serviceResult = await UpdateAsync();
                else if (CrudOperation.Value == EnumCrudOperation.DeleteOperation)
                    serviceResult = await DeleteAsync();
            }

            return serviceResult;
        }

        public virtual async Task<ServiceResult<TResult>> CreateAsync()
        {
            if (RunWithTransaction)
                TransactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = await DataLayer.AddAsync(Entity);
                if (resultDAL.IsSucceeded)
                {
                    result.IsSucceeded = true;
                }
                else
                {
                    result.ErrorMessage = resultDAL.ErrorMessage;
                    result.ExceptionMessage = resultDAL.ExceptionMessage;
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

        public virtual async Task<ServiceResult<TResult>> UpdateAsync()
        {
            if (RunWithTransaction)
                TransactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = await DataLayer.UpdateAsync(Entity);
                if (resultDAL.IsSucceeded)
                {
                    result.IsSucceeded = true;
                }
                else
                {
                    result.ErrorMessage = resultDAL.ErrorMessage;
                    result.ExceptionMessage = resultDAL.ExceptionMessage;
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

        public virtual async Task<ServiceResult<TResult>> DeleteAsync()
        {
            if (RunWithTransaction)
                TransactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = await DataLayer.DeleteAsync(Entity);
                if (resultDAL.IsSucceeded)
                {
                    result.IsSucceeded = true;
                }
                else
                {
                    result.ErrorMessage = resultDAL.ErrorMessage;
                    result.ExceptionMessage = resultDAL.ExceptionMessage;
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
