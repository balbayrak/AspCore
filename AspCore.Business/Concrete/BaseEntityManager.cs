using AspCore.Business.Abstract;
using AspCore.Business.Specifications.Abstract;
using AspCore.Business.Task.Abstract;
using AspCore.DataAccess.Abstract;
using AspCore.Dtos.Dto;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.Mapper.Abstract;
using AspCore.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspCore.Business.Concrete
{
    public abstract class BaseEntityManager<TDataAccess, TEntity, TEntityDto, TCreatedEntityDto, TUpdatedEntityDto> : IEntityService<
            TEntity, TEntityDto, TCreatedEntityDto, TUpdatedEntityDto>
        where TDataAccess : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TEntityDto : class, IEntityDto, new()
        where TCreatedEntityDto : class, IEntityDto, new()
        where TUpdatedEntityDto : class, IEntityDto, new()

    {
        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
            => LazyGetRequiredService(typeof(TService), ref reference);

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        protected IServiceProvider ServiceProvider { get; }
        protected readonly TDataAccess DataAccess;
        protected ITransactionBuilder TransactionBuilder;

        private IAutoObjectMapper _autoObjectMapper;
        protected IAutoObjectMapper AutoObjectMapper => LazyGetRequiredService(ref _autoObjectMapper);

        private ITaskBuilder _taskBuilder;
        protected ITaskBuilder TaskBuilder => LazyGetRequiredService(ref _taskBuilder);

        protected BaseEntityManager(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            DataAccess = ServiceProvider.GetRequiredService<TDataAccess>();
            TransactionBuilder = ServiceProvider.GetRequiredService<ITransactionBuilder>();
        }

        public virtual ServiceResult<bool> Add(params TCreatedEntityDto[] entities)
        {

            var entityArray = AutoObjectMapper.Mapper.Map<TCreatedEntityDto[], TEntity[]>(entities);
            if (entities.Length > 1)
            {
                return DataAccess.AddWithTransaction(entityArray);
            }
            else
            {
                return DataAccess.Add(entityArray);
            }
        }

        public virtual ServiceResult<bool> Update(params TUpdatedEntityDto[] entities)
        {
            var entityArray = AutoObjectMapper.Mapper.Map<TUpdatedEntityDto[], TEntity[]>(entities);

            if (entities.Length > 1)
                return DataAccess.UpdateWithTransaction(entityArray);
            else
                return DataAccess.Update(entityArray);
        }

        public virtual ServiceResult<bool> Delete(params Guid[] entityIds)
        {
            if (entityIds.Length > 1)
                return DataAccess.DeleteWithTransaction(entityIds);
            else
                return DataAccess.Delete(entityIds);
        }

        public virtual Task<ServiceResult<bool>> AddAsync(params TCreatedEntityDto[] entities)
        {
            var entityArray = AutoObjectMapper.Mapper.Map<TCreatedEntityDto[], TEntity[]>(entities);

            if (entities.Length > 1)
                return DataAccess.AddWithTransactionAsync(entityArray);
            else
                return DataAccess.AddAsync(entityArray);
        }

        public virtual Task<ServiceResult<bool>> UpdateAsync(params TUpdatedEntityDto[] entities)
        {
            var entityArray = AutoObjectMapper.Mapper.Map<TUpdatedEntityDto[], TEntity[]>(entities);

            if (entities.Length > 1)
                return DataAccess.UpdateWithTransactionAsync(entityArray);
            else
                return DataAccess.UpdateAsync(entityArray);
        }

        public virtual Task<ServiceResult<bool>> DeleteAsync(params Guid[] entityIds)
        {
            if (entityIds.Length > 1)
                return DataAccess.DeleteWithTransactionAsync(entityIds);
            else
                return DataAccess.DeleteAsync(entityIds);
        }

        public ServiceResult<TEntityDto> GetById(EntityFilter setting)
        {

            var data = DataAccess.GetById(setting.id);
            if (data.IsSucceeded)
            {
                var entityArray = AutoObjectMapper.Mapper.Map<TEntity, TEntityDto>(data.Result);
                return data.ChangeResult(entityArray);
            }

            return data.ChangeResult(default(TEntityDto));
        }

        public async Task<ServiceResult<TEntityDto>> GetByIdAsync(EntityFilter setting)
        {

            var data = await DataAccess.GetByIdAsync(setting.id);
            if (data.IsSucceeded)
            {
                var entityArray = AutoObjectMapper.Mapper.Map<TEntity, TEntityDto>(data.Result);
                return data.ChangeResult(entityArray);
            }

            return data.ChangeResult(default(TEntityDto));
        }

        public ServiceResult<IList<TEntityDto>> GetAll(EntityFilter setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            ServiceResult<IList<TEntity>> dataList;
            Expression<Func<TEntity, bool>> expression = null;
            List<SearchInfo> searchInfos = setting.GetSearchInfo();

            if (searchInfos != null && !string.IsNullOrEmpty(setting.search.searchValue))
            {
                expression = ExpressionBuilder.GetSearchExpression<TEntity>(searchInfos, setting.search.searchValue);
            }

            if (setting.sorters != null)
            {
                List<SortingExpression<TEntity>> sorters = null;
                if (setting.sorters != null)
                {
                    sorters = setting.sorters.ToSortingExpressionList<TEntity>();
                }

                dataList = DataAccess.FindList(expression, sorters, setting.page, setting.pageSize);
            }
            else
            {
                dataList = DataAccess.GetList(expression, setting.page, setting.pageSize);
            }

            var result = AutoObjectMapper.Mapper.Map<IList<TEntityDto>>(dataList.Result);
            return dataList.ChangeResult(result);
        }

        public async Task<ServiceResult<IList<TEntityDto>>> GetAllAsync(EntityFilter setting)
        {
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }
            ServiceResult<IList<TEntity>> dataList;
            Expression<Func<TEntity, bool>> expression = null;
            List<SearchInfo> searchInfos = setting.GetSearchInfo();

            if (searchInfos != null && !string.IsNullOrEmpty(setting.search.searchValue))
            {
                expression = ExpressionBuilder.GetSearchExpression<TEntity>(searchInfos, setting.search.searchValue);
            }

            if (setting.sorters != null)
            {
                List<SortingExpression<TEntity>> sorters = null;
                if (setting.sorters != null)
                {
                    sorters = setting.sorters.ToSortingExpressionList<TEntity>();
                }

                dataList = await DataAccess.FindListAsync(expression, sorters, setting.page, setting.pageSize);
            }
            else
            {
                dataList = await DataAccess.GetListAsync(expression, setting.page, setting.pageSize);
            }
            var result = AutoObjectMapper.Mapper.Map<IList<TEntityDto>>(dataList.Result);
            return dataList.ChangeResult(result);
        }

        public async Task<ServiceResult<List<TEntityDto>>> GetHistoriesByIdAsync(EntityFilter setting)
        {
            var data = await DataAccess.GetHistoriesById(setting.id, setting.page, setting.pageSize);
            var result = AutoObjectMapper.Mapper.Map<List<TEntityDto>>(data.Result);
            return data.ChangeResult(result);

        }

        public async Task<ServiceResult<IList<TEntityDto>>> GetAllAsync(ISpecification<TEntity> specification)
        {
            var data = await DataAccess.GetListAsync(specification.ToExpression());
            var result = AutoObjectMapper.Mapper.Map<IList<TEntityDto>>(data.Result);
            return data.ChangeResult(result);
        }


    }

    public abstract class BaseEntityManager<TDataAccess, TEntity, TEntityDto> : BaseEntityManager<TDataAccess, TEntity, TEntityDto,
            TEntityDto, TEntityDto>
        where TEntityDto : class, IEntityDto, new()
        where TDataAccess : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        protected BaseEntityManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }


}
