using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.DataAccess.Abstract;
using AspCore.DataSearch.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.Business.Concrete
{
    public abstract class BaseComplexSearchableEntityManager<TDataAccess, TEntity, TSearchableEntity> : BaseEntityManager<TDataAccess, TEntity>, IComplexSearchableEntityService<TEntity, TSearchableEntity>
        where TDataAccess : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TSearchableEntity : class, ISearchableEntity, new()
    {
        private readonly IDataSearchEngine<TSearchableEntity> _dataSearchEngine;

        public BaseComplexSearchableEntityManager()
        {
            _dataSearchEngine = DependencyResolver.Current.GetService<IDataSearchEngine<TSearchableEntity>>();
        }
        public abstract ServiceResult<TSearchableEntity> GetComplexEntity(TEntity entity);

        private ServiceResult<TSearchableEntity[]> GetComplexEntities(TEntity[] entities)
        {
            ServiceResult<TSearchableEntity[]> serviceResult = new ServiceResult<TSearchableEntity[]>();
            try
            {
                List<TSearchableEntity> list = new List<TSearchableEntity>();
                foreach (var item in entities)
                {
                    ServiceResult<TSearchableEntity> entityResult = GetComplexEntity(item);
                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        list.Add(entityResult.Result);
                    }
                    else
                    {
                        serviceResult.Result = null;
                        serviceResult.ErrorMessage = entityResult.ErrorMessage;
                        serviceResult.ExceptionMessage = entityResult.ExceptionMessage;
                        break;
                    }
                    entityResult = null;
                }

                if(string.IsNullOrEmpty(serviceResult.ErrorMessage))
                {
                    serviceResult.Result = list.ToArray();
                }
            }
            catch (Exception ex)
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
                    ServiceResult<TSearchableEntity[]> entityResult = GetComplexEntities(entities);
                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        ServiceResult<bool> resultCache = _dataSearchEngine.Create(entityResult.Result.ToArray());
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
                    ServiceResult<TSearchableEntity[]> entityResult = GetComplexEntities(entities);
                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        ServiceResult<bool> resultCache = _dataSearchEngine.Update(entityResult.Result.ToArray());
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
                    ServiceResult<TSearchableEntity[]> entityResult = GetComplexEntities(entityListResult.Result.ToArray());
                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        ServiceResult<bool> resultDAL = _dataAccess.Delete(entityIds);
                        if (resultDAL.IsSucceeded)
                        {
                            ServiceResult<bool> resultCache = _dataSearchEngine.Delete(entityResult.Result.ToArray());
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

        public ServiceResult<TSearchableEntity[]> GetSearchableEntities()
        {
            ServiceResult<TEntity[]> result = _dataAccess.GetListWithIgnoreGlobalFilter();
            if (result.IsSucceededAndDataIncluded())
            {
                return GetComplexEntities(result.Result);
            }
            return new ServiceResult<TSearchableEntity[]>
            {
                IsSucceeded = false,
                Result = null,
                ErrorMessage = result.ErrorMessage,
                ExceptionMessage = result.ExceptionMessage
            };
        }
    }
}
