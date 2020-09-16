using AspCore.DataAccess.Abstract;
using AspCore.DataAccess.EntityFramework.History;
using AspCore.DataAccess.General;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspCore.DataAccess.EntityFramework
{
    public abstract class EfEntityRepositoryBase<TDbContext, TEntity> : IEntityRepository<TEntity>
     where TEntity : class, IEntity,new()
        where TDbContext : CoreDbContext
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        protected TDbContext Context { get; }
        private DbSet<TEntity> _entities;

        protected virtual DbSet<TEntity> Entities => _entities ?? (_entities = Context.Set<TEntity>());
        protected virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        public EfEntityRepositoryBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Context = ServiceProvider.GetService<TDbContext>();
            _entities = Context.Set<TEntity>();
        }

        public ServiceResult<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            ServiceResult<TEntity> result = new ServiceResult<TEntity>();
            try
            {
                result.Result = Entities.FirstOrDefault(filter);
                result.IsSucceeded = true;

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public async Task<ServiceResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            ServiceResult<TEntity> result = new ServiceResult<TEntity>();
            try
            {
                result.Result = await Entities.FirstOrDefaultAsync(filter);
                result.IsSucceeded = true;

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<IList<TEntity>> GetList(DataAccessFilter<TEntity> dataAccessFilter = null)
        {
            ServiceResult<IList<TEntity>> result = new ServiceResult<IList<TEntity>>();
            try
            {
                IQueryable<TEntity> query;
                int countTask;
                int searchTask;

                if (dataAccessFilter != null)
                {
                    if (dataAccessFilter.includes != null)
                    {
                        query = WithIncludes(dataAccessFilter.includes);
                    }
                    else
                        query = TableNoTracking.AsQueryable();

                    if (dataAccessFilter.query != null)
                        query = query.Where(dataAccessFilter.query);

                    countTask = query.Count();

                    if (dataAccessFilter.searchQuery != null)
                    {
                        if (dataAccessFilter.query != null)
                            query = query.Where(dataAccessFilter.query.CombineWithAndAlso(dataAccessFilter.searchQuery));
                        else
                            query = query.Where(dataAccessFilter.searchQuery);
                    }

                    if (dataAccessFilter.page.HasValue && dataAccessFilter.page.Value >= 0 && dataAccessFilter.pageSize.HasValue)
                    {
                        searchTask = query.Count();

                        var skip = dataAccessFilter.page.Value * dataAccessFilter.pageSize.Value;

                        query = query.Skip(skip).Take(dataAccessFilter.pageSize.Value);
                    }
                    else
                        searchTask = query.Count();
                }
                else
                {
                    query = TableNoTracking.AsQueryable();
                    countTask = query.Count();
                    searchTask = query.Count();
                }

                result.Result = query.ToArray();
                result.TotalResultCount = countTask == searchTask ? countTask : searchTask;
                result.IsSucceeded = true;
                result.SearchResultCount = searchTask;

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public async Task<ServiceResult<IList<TEntity>>> GetListAsync(DataAccessFilter<TEntity> dataAccessFilter = null)
        {
            ServiceResult<IList<TEntity>> result = new ServiceResult<IList<TEntity>>();
            try
            {
                IQueryable<TEntity> query;
                int countTask;
                int searchTask;
                if (dataAccessFilter != null)
                {
                    if (dataAccessFilter.includes != null)
                    {
                        query = WithIncludes(dataAccessFilter.includes);
                    }
                    else
                        query = TableNoTracking.AsQueryable();

                    if (dataAccessFilter.query != null)
                        query = query.Where(dataAccessFilter.query);

                    countTask = await query.CountAsync();

                    if (dataAccessFilter.searchQuery != null)
                    {
                        if (dataAccessFilter.query != null)
                            query = query.Where(dataAccessFilter.query.CombineWithAndAlso(dataAccessFilter.searchQuery));
                        else
                            query = query.Where(dataAccessFilter.searchQuery);
                    }

                    if (dataAccessFilter.page.HasValue && dataAccessFilter.page.Value >= 0 && dataAccessFilter.pageSize.HasValue)
                    {
                        searchTask = await query.CountAsync();

                        var skip = dataAccessFilter.page.Value * dataAccessFilter.pageSize.Value;

                        query = query.Skip(skip).Take(dataAccessFilter.pageSize.Value);
                    }
                    else
                        searchTask = await query.CountAsync();

                }
                else
                {
                    query = TableNoTracking.AsQueryable();
                    countTask = await query.CountAsync();
                    searchTask = await query.CountAsync();
                }

                result.Result = await query.ToArrayAsync();
                result.TotalResultCount = countTask == searchTask ? countTask : searchTask;
                result.IsSucceeded = true;
                result.SearchResultCount = searchTask;
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        private IQueryable<TEntity> WithIncludes(List<Expression<Func<TEntity, object>>> propertySelectors)
        {
            var query = TableNoTracking;

            if (propertySelectors != null && propertySelectors.Count > 0)
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public ServiceResult<TEntity[]> GetListWithIgnoreGlobalFilter()
        {
            ServiceResult<TEntity[]> result = new ServiceResult<TEntity[]>();
            try
            {
                var query = Entities.IgnoreQueryFilters().AsQueryable();

                var countTask = query.Count();

                result.Result = query.ToArray();

                result.IsSucceeded = true;
                result.TotalResultCount = countTask;
                result.SearchResultCount = result.Result.Count();

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public async Task<ServiceResult<TEntity[]>> GetListWithIgnoreGlobalFilterAsync()
        {
            ServiceResult<TEntity[]> result = new ServiceResult<TEntity[]>();
            try
            {
                var query = Entities.IgnoreQueryFilters().AsQueryable();

                var countTask = query.Count();

                result.Result = await query.ToArrayAsync();

                result.IsSucceeded = true;
                result.TotalResultCount = countTask;
                result.SearchResultCount = result.Result.Count();

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        /// <summary>
        /// work without transaction
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public ServiceResult<bool> Add(params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (TEntity item in entities)
                {
                    Context.Entry(item).State = EntityState.Added;
                    Context.Set<TEntity>().Add(item);
                }

                result = ProcessEntityWithStateNotTransaction(EntityState.Added, entities);

                if (result.IsSucceeded && result.Result)
                {
                    result.StatusMessage<bool, TEntity>(DALConstants.DALErrorMessages.DAL_ADD_SUCCESS_MESSAGE_WITH_PARAMETER, CoreEntityState.Added);
                }
                else
                {
                    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }


            return result;
        }

        /// <summary>
        /// work without transaction
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> AddAsync(params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (TEntity item in entities)
                {
                    item.entityState = CoreEntityState.Added;
                    Context.Set<TEntity>().Add(item);
                }

                result = await ProcessEntityWithStateNotTransactionAsync(EntityState.Added, entities);

                if (result.IsSucceeded && result.Result)
                {
                    result.StatusMessage<bool, TEntity>(DALConstants.DALErrorMessages.DAL_ADD_SUCCESS_MESSAGE_WITH_PARAMETER, CoreEntityState.Added);
                }
                else
                {
                    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }


            return result;
        }

        /// <summary>
        /// work with transaction
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public ServiceResult<bool> AddWithTransaction(params TEntity[] entities)
        {
            foreach (var item in entities)
            {
                item.entityState = CoreEntityState.Added;
                Context.Set<TEntity>().Add(item);
            }

            ServiceResult<bool> result = ProcessEntityWithState(EntityState.Added, entities);
            if (result.IsSucceeded)
            {
                result.StatusMessage<bool, TEntity>(DALConstants.DALErrorMessages.DAL_ADD_SUCCESS_MESSAGE_WITH_PARAMETER, CoreEntityState.Added);
            }

            return result;
        }

        /// <summary>
        /// work with transaction
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<ServiceResult<bool>> AddWithTransactionAsync(params TEntity[] entities)
        {
            foreach (var item in entities)
            {
                item.entityState = CoreEntityState.Added;
                Context.Set<TEntity>().Add(item);
            }

            ServiceResult<bool> result = await ProcessEntityWithStateAsync(EntityState.Added, entities);
            if (result.IsSucceeded)
            {
                result.StatusMessage<bool, TEntity>(DALConstants.DALErrorMessages.DAL_ADD_SUCCESS_MESSAGE_WITH_PARAMETER, CoreEntityState.Added);
            }

            return result;
        }

        public ServiceResult<bool> Update(params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (var updatedInput in entities)
                {
                    updatedInput.entityState = CoreEntityState.Modified;
                    Context.Update(updatedInput);
                }

                result = ProcessEntityWithStateNotTransaction(EntityState.Modified, entities);

                if (result.IsSucceeded && result.Result)
                {
                    result.StatusMessage<bool, TEntity>(DALConstants.DALErrorMessages.DAL_UPDATE_SUCCESS_MESSAGE_WITH_PARAMETER, CoreEntityState.Modified);
                }
                else
                {
                    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }


            return result;
        }

        public async Task<ServiceResult<bool>> UpdateAsync(params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (var updatedInput in entities)
                {
                    updatedInput.entityState = CoreEntityState.Modified;
                    Context.Update(updatedInput);
                }

                result = await ProcessEntityWithStateNotTransactionAsync(EntityState.Modified, entities);

                if (result.IsSucceeded && result.Result)
                {
                    result.StatusMessage<bool, TEntity>(DALConstants.DALErrorMessages.DAL_UPDATE_SUCCESS_MESSAGE_WITH_PARAMETER, CoreEntityState.Modified);
                }
                else
                {
                    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }


            return result;
        }

        public ServiceResult<bool> UpdateWithTransaction(params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (var updatedInput in entities)
                {
                    updatedInput.entityState = CoreEntityState.Modified;
                    Context.Update(updatedInput);
                }

                result = ProcessEntityWithState(EntityState.Modified, entities);

                if (result.IsSucceeded && result.Result)
                {
                    result.StatusMessage<bool, TEntity>(DALConstants.DALErrorMessages.DAL_UPDATE_SUCCESS_MESSAGE_WITH_PARAMETER, CoreEntityState.Modified);
                }
                else
                {
                    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public async Task<ServiceResult<bool>> UpdateWithTransactionAsync(params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (var updatedInput in entities)
                {
                    updatedInput.entityState = CoreEntityState.Modified;
                    Context.Update(updatedInput);
                }

                result = await ProcessEntityWithStateAsync(EntityState.Modified, entities);

                if (result.IsSucceeded && result.Result)
                {
                    result.StatusMessage<bool, TEntity>(DALConstants.DALErrorMessages.DAL_UPDATE_SUCCESS_MESSAGE_WITH_PARAMETER, CoreEntityState.Modified);
                }
                else
                {
                    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> Delete(params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (TEntity item in entities)
                {
                    var updatedEntity = Context.Entry(item);
                    updatedEntity.State = EntityState.Deleted;
                }
                result = ProcessEntityWithStateNotTransaction(EntityState.Deleted, entities);

                if (result.IsSucceeded && result.Result)
                {
                    result.StatusMessage<bool, TEntity>(DALConstants.DALErrorMessages.DAL_DELETE_SUCCESS_MESSAGE_WITH_PARAMETER, CoreEntityState.Modified);
                }
                else
                {
                    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public async Task<ServiceResult<bool>> DeleteAsync(params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (TEntity item in entities)
                {
                    var updatedEntity = Context.Entry(item);
                    updatedEntity.State = EntityState.Deleted;
                }

                result = await ProcessEntityWithStateNotTransactionAsync(EntityState.Deleted, entities);

                if (result.IsSucceeded && result.Result)
                {
                    result.StatusMessage<bool, TEntity>(DALConstants.DALErrorMessages.DAL_DELETE_SUCCESS_MESSAGE_WITH_PARAMETER, CoreEntityState.Modified);
                }
                else
                {
                    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> Delete(params Guid[] entityIds)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<List<TEntity>> entitylist = GetByIdListTracking(entityIds.ToList());
                if (entitylist.IsSucceededAndDataIncluded())
                {
                    result = Delete(entitylist.Result.ToArray());
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public async Task<ServiceResult<bool>> DeleteAsync(params Guid[] entityIds)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<List<TEntity>> entitylist = GetByIdListTracking(entityIds.ToList());
                if (entitylist.IsSucceededAndDataIncluded())
                {
                    result = await DeleteAsync(entitylist.Result.ToArray());
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> DeleteWithTransaction(params Guid[] entityIds)
        {
            ServiceResult<List<TEntity>> entitylist = GetByIdListTracking(entityIds.ToList());
            if (entitylist.IsSucceededAndDataIncluded())
            {
                entitylist.Result.ForEach(t => t.entityState = CoreEntityState.Deleted);
            }

            ServiceResult<bool> result = ProcessEntityWithState(EntityState.Deleted, entitylist.Result.ToArray());
            if (result.IsSucceeded)
            {
                result.StatusMessage<bool, TEntity>(DALConstants.DALErrorMessages.DAL_DELETE_SUCCESS_MESSAGE_WITH_PARAMETER, CoreEntityState.Deleted);
            }
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteWithTransactionAsync(params Guid[] entityIds)
        {
            ServiceResult<List<TEntity>> entitylist = await GetByIdListTrackingAsync(entityIds.ToList());
            if (entitylist.IsSucceededAndDataIncluded())
            {
                entitylist.Result.ForEach(t => t.entityState = CoreEntityState.Deleted);
            }

            ServiceResult<bool> result = await ProcessEntityWithStateAsync(EntityState.Deleted, entitylist.Result.ToArray());
            if (result.IsSucceeded)
            {
                result.StatusMessage<bool, TEntity>(DALConstants.DALErrorMessages.DAL_DELETE_SUCCESS_MESSAGE_WITH_PARAMETER, CoreEntityState.Deleted);
            }
            return result;
        }

        public ServiceResult<TEntity> Find(Expression<Func<TEntity, bool>> filter)
        {
            ServiceResult<TEntity> result = new ServiceResult<TEntity>();

            try
            {
                result.Result = Entities.Where(filter).FirstOrDefault();
                result.IsSucceeded = true;

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public async Task<ServiceResult<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            ServiceResult<TEntity> result = new ServiceResult<TEntity>();

            try
            {
                result.Result = await Entities.Where(filter).FirstOrDefaultAsync();
                result.IsSucceeded = true;

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<TEntity> GetById(Guid id)
        {
            ServiceResult<TEntity> result = new ServiceResult<TEntity>();

            try
            {
                result.Result = Entities.Find(id);
                result.IsSucceeded = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public async Task<ServiceResult<TEntity>> GetByIdAsync(Guid id)
        {
            ServiceResult<TEntity> result = new ServiceResult<TEntity>();

            try
            {
                result.Result = await Entities.FindAsync(id);
                result.IsSucceeded = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public async Task<ServiceResult<List<TEntity>>> GetHistoriesById(Guid id, int? page = null, int? pageSize = null)
        {
            ServiceResult<List<TEntity>> result = new ServiceResult<List<TEntity>>();

            try
            {
                if (typeof(TEntity).IsAssignableTo(typeof(IAutoHistory)))
                {
                    List<TEntity> entities = new List<TEntity>();

                    List<AspCoreAutoHistory> list = await Context.Set<AspCoreAutoHistory>().AsNoTracking().Where(t => t.RowId.Equals(id.ToString())).OrderByDescending(t => t.Changed).ToListAsync();
                    if (list != null && list.Count > 0)
                    {
                        int start = 0;
                        int count = list.Count;

                        if (page.HasValue && page.Value >= 0 && pageSize.HasValue)
                        {
                            start = page.Value * pageSize.Value;
                            count = pageSize.Value;
                        }

                        for (int i = start; i < count; i++)
                        {
                            JObject obj = (JObject)JsonConvert.DeserializeObject(list[i].Changed);
                            var historyChanged = obj.ToObject<EntityHistoryChanged<TEntity>>();
                            if (i == 0)
                            {
                                if (typeof(TEntity).IsAssignableTo(typeof(IBaseEntity)))
                                {
                                    ((IBaseEntity)historyChanged.before).LastUpdateDate = list[i].Created;
                                }
                                entities.Add(historyChanged.before);
                                entities.Add(historyChanged.after);
                            }
                            else
                            {
                                entities.Add(historyChanged.after);
                            }
                        }
                        result.Result = entities;
                        result.IsSucceeded = true;
                        result.TotalResultCount = entities.Count;
                        result.SearchResultCount = pageSize.HasValue ? pageSize.Value + 1 : entities.Count;
                    }
                    else
                    {
                        result.IsSucceeded = false;
                        result.Result = null;
                    }
                }
                else
                {
                    result.IsSucceeded = false;
                    result.Result = null;
                    result.ErrorMessage = DALConstants.DALErrorMessages.ENTITY_IS_NOT_IAUTOHISTORY;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<List<TEntity>> GetByIdList(params Guid[] entityIds)
        {
            ServiceResult<List<TEntity>> result = new ServiceResult<List<TEntity>>();

            try
            {
                result.Result = Entities.Where(t => entityIds.Any(p => p == t.Id)).ToList();
                result.IsSucceeded = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public async Task<ServiceResult<List<TEntity>>> GetByIdListAsync(params Guid[] entityIds)
        {
            ServiceResult<List<TEntity>> result = new ServiceResult<List<TEntity>>();

            try
            {
                result.Result = await Entities.Where(t => entityIds.Any(p => p == t.Id)).ToListAsync();
                result.IsSucceeded = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        private ServiceResult<List<TEntity>> GetByIdListTracking(List<Guid> idList)
        {
            ServiceResult<List<TEntity>> result = new ServiceResult<List<TEntity>>();

            try
            {
                result.Result = Entities.Where(t => idList.Any(p => p == t.Id)).ToList();
                result.IsSucceeded = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        private async Task<ServiceResult<List<TEntity>>> GetByIdListTrackingAsync(List<Guid> idList)
        {
            ServiceResult<List<TEntity>> result = new ServiceResult<List<TEntity>>();

            try
            {
                result.Result = await Entities.Where(t => idList.Any(p => p == t.Id)).ToListAsync();
                result.IsSucceeded = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        private ServiceResult<bool> ProcessEntityWithState(EntityState defaultState, params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();

            if (entities != null)
            {
                IDbContextTransaction dbContextTransaction = null;
                try
                {
                    using (dbContextTransaction = Context.Database.BeginTransaction())
                    {
                        foreach (EntityEntry<IEntity> entry in Context.ChangeTracker.Entries<IEntity>())
                        {
                            IEntity entity = entry.Entity;

                            if (entity.entityState.HasValue)
                                entry.State = GetEntityState(entity.entityState.Value);
                            else
                                entry.State = defaultState;
                           
                            Context.Entry(entity);
                        }

                        int value = Context.SaveChanges();
                        if (value > 0)
                        {
                            dbContextTransaction.Commit();
                            result.IsSucceeded = true;
                            result.Result = true;
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (dbContextTransaction != null) dbContextTransaction.Rollback();
                    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
                }
            }

            return result;
        }

        private async Task<ServiceResult<bool>> ProcessEntityWithStateAsync(EntityState defaultState, params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();

            if (entities != null)
            {
                IDbContextTransaction dbContextTransaction = null;
                try
                {
                    using (dbContextTransaction = Context.Database.BeginTransaction())
                    {
                        foreach (EntityEntry<IEntity> entry in Context.ChangeTracker.Entries<IEntity>())
                        {
                            IEntity entity = entry.Entity;
                            if (entity.entityState.HasValue)
                                entry.State = GetEntityState(entity.entityState.Value);
                            else
                                entry.State = defaultState;
                            Context.Entry(entity);
                        }

                        int value = await Context.SaveChangesAsync();
                        if (value > 0)
                        {
                            dbContextTransaction.Commit();
                            result.IsSucceeded = true;
                            result.Result = true;
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (dbContextTransaction != null) dbContextTransaction.Rollback();
                    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
                }
            }

            return result;
        }

        private ServiceResult<bool> ProcessEntityWithStateNotTransaction(EntityState defaultState, TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (EntityEntry<IEntity> entry in Context.ChangeTracker.Entries<IEntity>())
                {
                    IEntity entity = entry.Entity;
                    if (entity.entityState.HasValue)
                        entry.State = GetEntityState(entity.entityState.Value);
                    else
                        entry.State = defaultState;
                    Context.Entry(entity);
                }

                int value = Context.SaveChanges();
                if (value > 0)
                {
                    result.IsSucceeded = true;
                    result.Result = true;
                }
                else
                {
                    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        private async Task<ServiceResult<bool>> ProcessEntityWithStateNotTransactionAsync(EntityState defaultState, TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {

                foreach (EntityEntry<IEntity> entry in Context.ChangeTracker.Entries<IEntity>())
                {
                    IEntity entity = entry.Entity;
                    if (entity.entityState.HasValue)
                        entry.State = GetEntityState(entity.entityState.Value);
                    else
                        entry.State = defaultState;
                    Context.Entry(entity);
                }

                int value = await Context.SaveChangesAsync();
                if (value > 0)
                {
                    result.IsSucceeded = true;
                    result.Result = true;
                }
                else
                {
                    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        private ServiceResult<bool> ProcessEntitiesWithState(EntityState defaultState, List<TEntity> items)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            IDbContextTransaction dbContextTransaction = null;
            try
            {
                using (dbContextTransaction = Context.Database.BeginTransaction())
                {
                    foreach (EntityEntry<IEntity> entry in Context.ChangeTracker.Entries<IEntity>())
                    {
                        IEntity entity = entry.Entity;
                        if (entity.entityState.HasValue)
                            entry.State = GetEntityState(entity.entityState.Value);
                        else
                            entry.State = defaultState;
                        Context.Entry(entity);
                    }

                    int value = Context.SaveChanges();
                    if (value > 0)
                    {
                        dbContextTransaction.Commit();
                        result.IsSucceeded = true;
                        result.Result = true;
                    }
                    else
                    {
                        dbContextTransaction.Rollback();
                        result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                    }
                }
            }
            catch (Exception ex)
            {
                if (dbContextTransaction != null) dbContextTransaction.Rollback();
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;

        }

        private EntityState GetEntityState(CoreEntityState entityState)
        {
            switch (entityState)
            {
                case CoreEntityState.Unchanged:
                    return EntityState.Unchanged;
                case CoreEntityState.Added:
                    return EntityState.Added;
                case CoreEntityState.Modified:
                    return EntityState.Modified;
                case CoreEntityState.Deleted:
                    return EntityState.Deleted;
                case CoreEntityState.Detached:
                    return EntityState.Detached;
                default:
                    return EntityState.Detached;
            }
        }

    }
}
