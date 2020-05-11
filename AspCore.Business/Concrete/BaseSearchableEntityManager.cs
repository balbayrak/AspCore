using AspCore.Business.Abstract;
using AspCore.DataAccess.Abstract;
using AspCore.DataSearch.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace AspCore.Business.Concrete
{
    public abstract class BaseSearchableEntityManager<TDataAccess, TSearchableEntity, TDataSearchEngine> : BaseEntityManager<TDataAccess, TSearchableEntity>, ISearchableEntityService<TSearchableEntity>, IEntityService<TSearchableEntity>
      where TDataAccess : IEntityRepository<TSearchableEntity>
      where TSearchableEntity : class, ISearchableEntity, new()
      where TDataSearchEngine : IDataSearchEngine<TSearchableEntity>
    {
        private readonly TDataSearchEngine _dataSearchEngine;

        public BaseSearchableEntityManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _dataSearchEngine = ServiceProvider.GetRequiredService<TDataSearchEngine>();
        }

        public override ServiceResult<bool> Add(params TSearchableEntity[] entities)
        {
            _transactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<bool> resultDAL = _dataAccess.Add(entities);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = _dataSearchEngine.Create(entities);
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

        public override ServiceResult<bool> Update(params TSearchableEntity[] entities)
        {
            _transactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<bool> resultDAL = _dataAccess.Update(entities);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = _dataSearchEngine.Update(entities);
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
                ServiceResult<List<TSearchableEntity>> entityListResult = _dataAccess.GetByIdList(entityIds);

                if (entityListResult.IsSucceededAndDataIncluded())
                {
                    ServiceResult<bool> resultDAL = _dataAccess.Delete(entityIds);
                    if (resultDAL.IsSucceeded)
                    {
                        ServiceResult<bool> resultCache = _dataSearchEngine.Delete(entityListResult.Result.ToArray());
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

        public ServiceResult<TSearchableEntity[]> GetSearchableEntities()
        {
            return _dataAccess.GetListWithIgnoreGlobalFilter();
        }

    }
}
