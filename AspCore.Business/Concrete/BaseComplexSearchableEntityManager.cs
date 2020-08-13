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
using AspCore.Dtos.Dto;
using System.Threading.Tasks;

namespace AspCore.Business.Concrete
{
    public abstract class BaseComplexSearchableEntityManager<TDataAccess, TEntity, TEntityDto, TCreatedDto, TUpdateDto, TSearchableEntity, TDataSearchEngine> : BaseEntityManager<TDataAccess, TEntity, TEntityDto, TCreatedDto, TUpdateDto>
        where TDataAccess : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TSearchableEntity : class, ISearchableEntity, new()
        where TDataSearchEngine : IDataSearchEngine<TSearchableEntity>
        where TEntityDto : class, IEntityDto, new()
        where TCreatedDto : class, IEntityDto, new()
        where TUpdateDto : class, IEntityDto, new()
    {
       
        private readonly TDataSearchEngine _dataSearchEngine;

        protected BaseComplexSearchableEntityManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _dataSearchEngine = ServiceProvider.GetService<TDataSearchEngine>();
        }

        public abstract Task<ServiceResult<TSearchableEntity>> GetComplexEntity(TEntity entity);

        private async Task<ServiceResult<TSearchableEntity[]>> GetComplexEntities(TEntity[] entities)
        {
            ServiceResult<TSearchableEntity[]> serviceResult = new ServiceResult<TSearchableEntity[]>();
            try
            {
                List<TSearchableEntity> list = new List<TSearchableEntity>();
                foreach (var item in entities)
                {
                    ServiceResult<TSearchableEntity> entityResult = await GetComplexEntity(item);
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

        public override async Task<ServiceResult<bool>> AddAsync(params TCreatedDto[] entities)
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var entitiesArray = AutoObjectMapper.Mapper.Map<TCreatedDto[], TEntity[]>(entities);
                ServiceResult<bool> resultDAL = await DataAccess.AddAsync(entitiesArray);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<TSearchableEntity[]> entityResult =await GetComplexEntities(entitiesArray);
                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        ServiceResult<bool> resultCache = await _dataSearchEngine.CreateAsync(entityResult.Result.ToArray());
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

        public override async Task<ServiceResult<bool>> UpdateAsync(params TUpdateDto[] entities)
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var entitiesArray = AutoObjectMapper.Mapper.Map<TUpdateDto[], TEntity[]>(entities);

                ServiceResult<bool> resultDAL = await DataAccess.UpdateAsync(entitiesArray);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<TSearchableEntity[]> entityResult = await GetComplexEntities(entitiesArray);
                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        ServiceResult<bool> resultCache = await  _dataSearchEngine.UpdateAsync(entityResult.Result.ToArray());
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

        public override async Task<ServiceResult<bool>> DeleteAsync(params Guid[] entityIds)
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<List<TEntity>> entityListResult = await DataAccess.GetByIdListAsync(entityIds);

                if (entityListResult.IsSucceededAndDataIncluded())
                {
                    ServiceResult<TSearchableEntity[]> entityResult = await GetComplexEntities(entityListResult.Result.ToArray());
                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        ServiceResult<bool> resultDAL =await DataAccess.DeleteAsync(entityIds);
                        if (resultDAL.IsSucceeded)
                        {
                            ServiceResult<bool> resultCache = await _dataSearchEngine.DeleteAsync(entityResult.Result.ToArray());
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

    public abstract class BaseComplexSearchableEntityManager<TDataAccess, TEntity, TEntityDto, TSearchableEntity, TDataSearchEngine> :
            BaseComplexSearchableEntityManager<TDataAccess, TEntity, TEntityDto, TEntityDto, TEntityDto,
                TSearchableEntity, TDataSearchEngine>
        where TDataAccess : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TSearchableEntity : class, ISearchableEntity, new()
        where TDataSearchEngine : IDataSearchEngine<TSearchableEntity>
        where TEntityDto : class, IEntityDto, new()
    {
        protected BaseComplexSearchableEntityManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
