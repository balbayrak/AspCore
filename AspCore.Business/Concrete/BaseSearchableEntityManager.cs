using AspCore.Business.Abstract;
using AspCore.DataAccess.Abstract;
using AspCore.DataSearch.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using AspCore.Dtos.Dto;
using System.Threading.Tasks;

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

        public override async Task<ServiceResult<bool>> AddAsync(params TSearchableEntityDto[] entities)
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var entitiesArray = AutoObjectMapper.Mapper.Map<TSearchableEntityDto[], TSearchableEntity[]>(entities);
                ServiceResult<bool> resultDAL = await DataAccess.AddAsync(entitiesArray);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = await _dataSearchEngine.CreateAsync(entitiesArray);
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

        public override async Task<ServiceResult<bool>> UpdateAsync(params TSearchableEntityDto[] entities)
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var entitiesArray = AutoObjectMapper.Mapper.Map<TSearchableEntityDto[], TSearchableEntity[]>(entities);

                ServiceResult<bool> resultDAL = await DataAccess.UpdateAsync(entitiesArray);
                if (resultDAL.IsSucceeded)
                {
                    ServiceResult<bool> resultCache = await _dataSearchEngine.UpdateAsync(entitiesArray);
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

        public override async Task<ServiceResult<bool>> DeleteAsync(params Guid[] entityIds)
        {
            TransactionBuilder.BeginTransaction();

            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<List<TSearchableEntity>> entityListResult = await DataAccess.GetByIdListAsync(entityIds);

                if (entityListResult.IsSucceededAndDataIncluded())
                {
                    ServiceResult<bool> resultDAL = await DataAccess.DeleteAsync(entityIds);
                    if (resultDAL.IsSucceeded)
                    {
                        ServiceResult<bool> resultCache = await _dataSearchEngine.DeleteAsync(entityListResult.Result.ToArray());
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
