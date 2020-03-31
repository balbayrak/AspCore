using AspCore.Business.Abstract;
using AspCore.CacheEntityClient;
using AspCore.DataAccess.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;

namespace AspCore.Business.Manager
{
    public class BaseCacheEntityManager<TDataAccess, TEntity> : BaseEntityManager<TDataAccess, TEntity>, ICacheEntityService<TEntity>
      where TDataAccess : IEntityRepository<TEntity>
      where TEntity : class, ICacheEntity, new()
    {
        private readonly ICacheClient<TEntity> _cacheClient;

        public BaseCacheEntityManager()
        {
            _cacheClient = DependencyResolver.Current.GetService<ICacheClient<TEntity>>();
        }

        public override ServiceResult<bool> Add(params TEntity[] entities)
        {
            _transactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<bool> resultDAL = _dataAccess.Add(entities);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = _cacheClient.Create(entities);
                    if (resultCache.IsSucceeded)
                    {
                        _transactionBuilder.CommitTransaction();
                        result.IsSucceeded = true;
                    }
                    else
                    {
                        result.ErrorMessage = resultCache.ErrorMessage;
                        result.ExceptionMessage = resultCache.ExceptionMessage;
                    }
                }
                else
                {
                    result.ErrorMessage = resultDAL.ErrorMessage;
                    result.ExceptionMessage = resultDAL.ExceptionMessage;
                }
            }
            catch
            {
                _transactionBuilder.RollbackTransaction();
            }
            finally
            {
                _transactionBuilder.DisposeTransaction();
            }

            return result;
        }

        public override ServiceResult<bool> Update(params TEntity[] entities)
        {
            _transactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<bool> resultDAL = _dataAccess.Update(entities);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = _cacheClient.Update(entities);
                    if (resultCache.IsSucceeded)
                    {
                        _transactionBuilder.CommitTransaction();
                        result.IsSucceeded = true;
                    }
                    else
                    {
                        result.ErrorMessage = resultCache.ErrorMessage;
                        result.ExceptionMessage = resultCache.ExceptionMessage;
                    }
                }
                else
                {
                    result.ErrorMessage = resultDAL.ErrorMessage;
                    result.ExceptionMessage = resultDAL.ExceptionMessage;
                }
            }
            catch
            {
                _transactionBuilder.RollbackTransaction();
            }
            finally
            {
                _transactionBuilder.DisposeTransaction();
            }

            return result;
        }

        public override ServiceResult<bool> Delete(params Guid[] entityIds)
        {
            _transactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<List<TEntity>> entityListResult = _dataAccess.GetByIdList(entityIds);

                if (entityListResult.IsSucceededAndDataIncluded())
                {
                    ServiceResult<bool> resultDAL = _dataAccess.Delete(entityIds);
                    if (resultDAL.IsSucceeded)
                    {
                        ServiceResult<bool> resultCache = _cacheClient.Delete(entityListResult.Result.ToArray());
                        if (resultCache.IsSucceeded)
                        {
                            _transactionBuilder.CommitTransaction();
                            result.IsSucceeded = true;
                        }
                        else
                        {
                            result.ErrorMessage = resultCache.ErrorMessage;
                            result.ExceptionMessage = resultCache.ExceptionMessage;
                        }
                    }
                    else
                    {
                        result.ErrorMessage = resultDAL.ErrorMessage;
                        result.ExceptionMessage = resultDAL.ExceptionMessage;
                    }
                }
                else
                {
                    result.ErrorMessage = entityListResult.ErrorMessage;
                    result.ExceptionMessage = entityListResult.ExceptionMessage;
                }
            }
            catch
            {
                _transactionBuilder.RollbackTransaction();
            }
            finally
            {
                _transactionBuilder.DisposeTransaction();
            }

            return result;
        }

    }
}
