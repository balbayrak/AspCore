using AspCore.Business.General;
using AspCore.DataAccess.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.Utilities.Mapper.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace AspCore.Business.Concrete
{
    public abstract class BaseSearchManager<TDataAccess, TEntity, TSearchableEntity>
            where TDataAccess : IEntityRepository<TEntity>
            where TEntity : class, IEntity, new()
            where TSearchableEntity : class, ISearchableEntity, new()
    {
        protected ICustomMapper Mapper { get; private set; }
        protected TDataAccess DataAccess;
        protected IServiceProvider ServiceProvider;

        protected BaseSearchManager(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            DataAccess = ServiceProvider.GetRequiredService<TDataAccess>();
            Mapper = ServiceProvider.GetRequiredService<ICustomMapper>();
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
       
        public ServiceResult<TSearchableEntity[]> GetSearchableEntities()
        {
            ServiceResult<TEntity[]> result = DataAccess.GetListWithIgnoreGlobalFilter();
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
