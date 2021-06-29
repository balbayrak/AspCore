﻿using AspCore.Business.Abstract;
using AspCore.Business.Specifications.Abstract;
using AspCore.DataAccess.Abstract;
using AspCore.DataAccess.General;
using AspCore.Dtos.Dto;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace AspCore.Business.Concrete
{
    public abstract class BaseEntityManager<TDataAccess, TEntity, TEntityDto, TCreatedEntityDto, TUpdatedEntityDto> : BaseBusinessManager, IEntityService<
            TEntity, TEntityDto, TCreatedEntityDto, TUpdatedEntityDto>
        where TDataAccess : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TEntityDto : class, IEntityDto, new()
        where TCreatedEntityDto : class, IEntityDto, new()
        where TUpdatedEntityDto : class, IEntityDto, new()

    {
       
        protected readonly TDataAccess DataAccess;
        protected BaseEntityManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            DataAccess = ServiceProvider.GetRequiredService<TDataAccess>();
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

        public virtual async Task<ServiceResult<bool>> UpdateAsync(TUpdatedEntityDto entityDto)
        {
            var entity = await DataAccess.GetByIdAsync(entityDto.Id);
            if (entity.IsSucceeded)
            {
                var data = AutoObjectMapper.Mapper.Map(entityDto, entity.Result);
                var response = await DataAccess.UpdateAsync(data);
                return response;
            }
            return entity.ChangeResult(false);
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

        public ServiceResult<TEntityDto> GetById(Guid id)
        {

            var data = DataAccess.GetById(id);
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

        public async Task<ServiceResult<TEntityDto>> GetByIdAsync(Guid id)
        {

            var data = await DataAccess.GetByIdAsync(id);
            if (data.IsSucceeded)
            {
                var entityArray = AutoObjectMapper.Mapper.Map<TEntity, TEntityDto>(data.Result);
                return data.ChangeResult(entityArray);
            }

            return data.ChangeResult(default(TEntityDto));
        }

        public ServiceResult<IList<TEntityDto>> GetAll(EntityFilter setting = null)
        {

            ServiceResult<IList<TEntity>> dataList;

            if (setting != null)
            {
                var datafilter = setting.ToDataFilter<TEntity>();

                dataList = DataAccess.GetList(datafilter);
            }
            else
            {
                dataList = DataAccess.GetList();
            }

            var result = AutoObjectMapper.Mapper.Map<IList<TEntityDto>>(dataList.Result);
            return dataList.ChangeResult(result);
        }

        public async Task<ServiceResult<IList<TEntityDto>>> GetAllAsync(EntityFilter setting=null)
        {
            
            ServiceResult<IList<TEntity>> dataList;
            if(setting!=null)
            {
                var datafilter = setting.ToDataFilter<TEntity>();

                dataList = await DataAccess.GetListAsync(datafilter);
            }
            else
            {
                dataList = await DataAccess.GetListAsync();
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
            var datafilter = new DataAccessFilter<TEntity>();
            datafilter.query = specification.ToExpression();
            var data = await DataAccess.GetListAsync(datafilter);
            var result = AutoObjectMapper.Mapper.Map<IList<TEntityDto>>(data.Result);
            return data.ChangeResult(result);
        }

        public async Task<ServiceResult<IList<TEntityDto>>> GetAllAsync()
        {
            var data = await DataAccess.GetListAsync();
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
