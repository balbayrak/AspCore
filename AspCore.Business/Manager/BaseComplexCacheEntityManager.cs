using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.CacheEntityClient;
using AspCore.DataAccess.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using System;
using System.Collections.Generic;

namespace AspCore.Business.Manager
{
    public abstract class BaseComplexCacheEntityManager<TDataAccess, TEntity, TCacheEntity> : BaseEntityManager<TDataAccess, TEntity>, IComplexCacheEntityService<TEntity, TCacheEntity>
        where TDataAccess : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TCacheEntity : class, ICacheEntity, new()
    {
        private readonly ICacheClient<TCacheEntity> _cacheClient;

        public BaseComplexCacheEntityManager()
        {
            _cacheClient = DependencyResolver.Current.GetService<ICacheClient<TCacheEntity>>();
        }
        public abstract ServiceResult<TCacheEntity> GetComplexEntity(TEntity entity);

        private ServiceResult<List<TCacheEntity>> GetComplexEntities(TEntity[] entities)
        {
            ServiceResult<List<TCacheEntity>> serviceResult = new ServiceResult<List<TCacheEntity>>();
            try
            {
                serviceResult.Result = new List<TCacheEntity>();
                foreach (var item in entities)
                {
                    ServiceResult<TCacheEntity> entityResult = GetComplexEntity(item);
                    if(entityResult.IsSucceededAndDataIncluded())
                    {
                        serviceResult.Result.Add(entityResult.Result);
                    }
                    else
                    {
                        serviceResult.Result = null;
                        serviceResult.ErrorMessage = entityResult.ErrorMessage;
                        serviceResult.ExceptionMessage = entityResult.ExceptionMessage;
                    }
                    entityResult = null;
                }
            }
            catch(Exception ex)
            {
                serviceResult.ErrorMessage(BusinessConstants.BaseExceptionMessages.CACHE_ENTITY_CONVERT_EXCEPTION, ex);
            }

            return serviceResult;
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
                    ServiceResult<List<TCacheEntity>> entityResult = GetComplexEntities(entities);
                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        ServiceResult<bool> resultCache = _cacheClient.Create(entityResult.Result.ToArray());
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
                        result.ErrorMessage = entityResult.ErrorMessage;
                        result.ExceptionMessage = entityResult.ExceptionMessage;
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
                    ServiceResult<List<TCacheEntity>> entityResult = GetComplexEntities(entities);
                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        ServiceResult<bool> resultCache = _cacheClient.Update(entityResult.Result.ToArray());
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
                        result.ErrorMessage = entityResult.ErrorMessage;
                        result.ExceptionMessage = entityResult.ExceptionMessage;
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
                    ServiceResult<List<TCacheEntity>> entityResult = GetComplexEntities(entityListResult.Result.ToArray());
                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        ServiceResult<bool> resultDAL = _dataAccess.Delete(entityIds);
                        if (resultDAL.IsSucceeded)
                        {
                            ServiceResult<bool> resultCache = _cacheClient.Delete(entityResult.Result.ToArray());
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
                        result.ErrorMessage = entityResult.ErrorMessage;
                        result.ExceptionMessage = entityResult.ExceptionMessage;
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
