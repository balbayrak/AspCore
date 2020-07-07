using AspCore.Business.General;
using AspCore.DataAccess.Abstract;
using AspCore.DataSearch.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AspCore.Business.Task.Abstract
{
    public abstract class SearchableEntityTask<TSearchableEntity, TResult, TDAL> : EntityTask<TSearchableEntity,TResult, TDAL>
        where TSearchableEntity : class, ISearchableEntity, new()
        where TDAL : IEntityRepository<TSearchableEntity>
    {
        private readonly IDataSearchEngine<TSearchableEntity> _dataSearchEngine;
        public SearchableEntityTask(IServiceProvider serviceProvider, TSearchableEntity entity, EnumCrudOperation enumCrudOperation) : base(serviceProvider,entity,enumCrudOperation)
        {
            _dataSearchEngine = ServiceProvider.GetRequiredService<IDataSearchEngine<TSearchableEntity>>();
        }

        public override  async Task<ServiceResult<TResult>> CreateAsync()
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = await DataLayer.AddAsync(Entity);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = _dataSearchEngine.Create(Entity);
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

        public override async Task<ServiceResult<TResult>> UpdateAsync()
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = await DataLayer.UpdateAsync(Entity);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = _dataSearchEngine.Update(Entity);
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

        public override async Task<ServiceResult<TResult>> DeleteAsync()
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<TResult> result = new ServiceResult<TResult>();
            try
            {
                ServiceResult<bool> resultDAL = await DataLayer.DeleteAsync(Entity);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = _dataSearchEngine.Delete(Entity);
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
