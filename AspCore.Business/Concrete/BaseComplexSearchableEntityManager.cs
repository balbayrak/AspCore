using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.DataAccess.Abstract;
using AspCore.DataSearch.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.Utilities.Mapper.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.Business.Concrete
{
    public abstract class BaseComplexSearchableEntityManager<TDataAccess, TEntity, TSearchableEntity, TDataSearchEngine> : BaseEntityManager<TDataAccess, TEntity>, IEntityService<TEntity>
        where TDataAccess : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TSearchableEntity : class, ISearchableEntity, new()
        where TDataSearchEngine : IDataSearchEngine<TSearchableEntity>
    {
        protected ICustomMapper Mapper { get; private set; }
        private readonly TDataSearchEngine _dataSearchEngine;

        protected BaseComplexSearchableEntityManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Mapper = ServiceProvider.GetRequiredService<ICustomMapper>();
            _dataSearchEngine = ServiceProvider.GetService<TDataSearchEngine>();
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

                if (string.IsNullOrEmpty(serviceResult.ErrorMessage))
                {
                    serviceResult.IsSucceeded = true;

                    serviceResult.Result = list.ToArray();
                }
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(BusinessConstants.BaseExceptionMessages.SEARCHABLE_ENTITY_CONVERT_EXCEPTION, ex);
            }

            return serviceResult;
        }

        public override ServiceResult<bool> Add(params TEntity[] entities)
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<bool> resultDAL = DataAccess.Add(entities);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<TSearchableEntity[]> entityResult = GetComplexEntities(entities);
                    if (entityResult.IsSucceededAndDataIncluded())
                    {

                        ServiceResult<bool> resultCache = _dataSearchEngine.Create(entityResult.Result.ToArray());
                        if (resultCache.IsSucceeded)
                        {
                            TransactionBuilder.CommitTransaction();
                            result.IsSucceeded = true;
                            result.StatusMessage = resultDAL.StatusMessage;
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
                TransactionBuilder.RollbackTransaction();
            }
            finally
            {
                TransactionBuilder.DisposeTransaction();
            }

            return result;
        }

        public override ServiceResult<bool> Update(params TEntity[] entities)
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<bool> resultDAL = DataAccess.Update(entities);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<TSearchableEntity[]> entityResult = GetComplexEntities(entities);
                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        ServiceResult<bool> resultCache = _dataSearchEngine.Update(entityResult.Result.ToArray());
                        if (resultCache.IsSucceeded)
                        {
                            TransactionBuilder.CommitTransaction();
                            result.IsSucceeded = true;
                            result.StatusMessage = resultDAL.StatusMessage;
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
                TransactionBuilder.RollbackTransaction();
            }
            finally
            {
                TransactionBuilder.DisposeTransaction();
            }

            return result;
        }

        public override ServiceResult<bool> Delete(params Guid[] entityIds)
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<List<TEntity>> entityListResult = DataAccess.GetByIdList(entityIds);

                if (entityListResult.IsSucceededAndDataIncluded())
                {
                    ServiceResult<TSearchableEntity[]> entityResult = GetComplexEntities(entityListResult.Result.ToArray());
                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        ServiceResult<bool> resultDAL = DataAccess.Delete(entityIds);
                        if (resultDAL.IsSucceeded)
                        {
                            ServiceResult<bool> resultCache = _dataSearchEngine.Delete(entityResult.Result.ToArray());
                            if (resultCache.IsSucceeded)
                            {
                                TransactionBuilder.CommitTransaction();
                                result.IsSucceeded = true;
                                result.StatusMessage = resultDAL.StatusMessage;
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
