using AspCore.Business.Abstract;
using AspCore.DataAccess.Abstract;
using AspCore.DataSearch.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using AspCore.Dtos.Dto;

namespace AspCore.Business.Concrete
{
    public abstract class BaseSearchableEntityManager<TDataAccess, TSearchableEntity, TSearchableEntityDto, TDataSearchEngine> : BaseEntityManager<TDataAccess, TSearchableEntity, TSearchableEntityDto>, IEntityService<TSearchableEntity, TSearchableEntityDto>
      where TDataAccess : IEntityRepository<TSearchableEntity>
      where TSearchableEntity : class, ISearchableEntity, new()
      where TDataSearchEngine : IDataSearchEngine<TSearchableEntity>
      where TSearchableEntityDto : class, ISearchableEntityDto, new()
    {
        private readonly TDataSearchEngine _dataSearchEngine;

        public BaseSearchableEntityManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _dataSearchEngine = ServiceProvider.GetRequiredService<TDataSearchEngine>();
        }

        public override ServiceResult<bool> Add(params TSearchableEntityDto[] entities)
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var entitiesArray = AutoObjectMapper.Mapper.Map< TSearchableEntityDto[], TSearchableEntity[]>(entities);
                ServiceResult<bool> resultDAL = DataAccess.Add(entitiesArray);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = _dataSearchEngine.Create(entitiesArray);
                    if (resultCache.IsSucceeded)
                    {
                        TransactionBuilder.CommitTransaction();
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
                TransactionBuilder.RollbackTransaction();
            }
            finally
            {
                TransactionBuilder.DisposeTransaction();
            }

            return result;
        }

        public override ServiceResult<bool> Update(params TSearchableEntityDto[] entities)
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var entitiesArray = AutoObjectMapper.Mapper.Map<TSearchableEntityDto[], TSearchableEntity[]>(entities);

                ServiceResult<bool> resultDAL = DataAccess.Update(entitiesArray);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = _dataSearchEngine.Update(entitiesArray);
                    if (resultCache.IsSucceeded)
                    {
                        TransactionBuilder.CommitTransaction();
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
                ServiceResult<List<TSearchableEntity>> entityListResult = DataAccess.GetByIdList(entityIds);

                if (entityListResult.IsSucceededAndDataIncluded())
                {
                    ServiceResult<bool> resultDAL = DataAccess.Delete(entityIds);
                    if (resultDAL.IsSucceeded)
                    {
                        ServiceResult<bool> resultCache = _dataSearchEngine.Delete(entityListResult.Result.ToArray());
                        if (resultCache.IsSucceeded)
                        {
                            TransactionBuilder.CommitTransaction();
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
