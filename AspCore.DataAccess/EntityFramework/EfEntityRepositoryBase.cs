﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspCore.DataAccess.Abstract;
using AspCore.DataAccess.General;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.Utilities;

namespace AspCore.DataAccess.EntityFramework
{
    public abstract class EfEntityRepositoryBase<TDbContext, TEntity> : IEntityRepository<TEntity>
     where TEntity : class, IEntity, new()
        where TDbContext : CoreDbContext
    {
        private TDbContext _context { get; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private DbSet<TEntity> _entities;
        private Guid activeUserId
        {
            get
            {
                return new Guid(_httpContextAccessor.HttpContext.GetHeaderValue(HttpContextConstant.HEADER_KEY.ACTIVE_USER_ID));
            }
        }
        protected virtual DbSet<TEntity> Entities => _entities ?? (_entities = _context.Set<TEntity>());
        protected virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();
        public EfEntityRepositoryBase()
        {
            _httpContextAccessor = DependencyResolver.Current.GetService<IHttpContextAccessor>();
            _context = DependencyResolver.Current.GetService<TDbContext>();
            _entities = _context.Set<TEntity>();
        }

        public ServiceResult<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            ServiceResult<TEntity> result = new ServiceResult<TEntity>();
            try
            {
                result.Result = TableNoTracking.FirstOrDefault(filter);
                result.IsSucceeded = true;

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<IList<TEntity>> GetList(Expression<Func<TEntity, bool>> filter, int? page = null, int? pageSize = null)
        {
            ServiceResult<IList<TEntity>> result = new ServiceResult<IList<TEntity>>();
            try
            {
                var query = TableNoTracking.AsQueryable();

                var countTask = query.Count();
                if (filter != null)
                    query = query.Where(filter);

                if (page.HasValue && page.Value >= 0 && pageSize.HasValue)
                {
                    var skip = page.Value * pageSize.Value;

                    result.Result = query.Skip(skip).Take(pageSize.Value).ToArray();
                }
                else
                {
                    result.Result = query.ToArray();
                }
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

        public async Task<ServiceResult<IList<TEntity>>> GetListAsync(Expression<Func<TEntity, bool>> filter, int? page = null, int? pageSize = null)
        {
            ServiceResult<IList<TEntity>> result = new ServiceResult<IList<TEntity>>();
            try
            {
                var query = TableNoTracking.AsQueryable();
                var countTask = await query.CountAsync();

                if (filter != null)
                    query = query.Where(filter);

                TEntity[] resultsTask;
                if (page.HasValue && page.Value >= 0 && pageSize.HasValue)
                {
                    var skip = page.Value * pageSize.Value;
                    resultsTask = await query.Skip(skip).Take(pageSize.Value).ToArrayAsync();
                }
                else
                {
                    resultsTask = await query.ToArrayAsync();
                }

                //   await Task.WhenAll(resultsTask, countTask).ConfigureAwait(false);

                //if (countTask.IsCompletedSuccessfully && resultsTask.IsCompletedSuccessfully)
                //{
                result.IsSucceeded = true;
                result.TotalResultCount = countTask;
                result.Result = resultsTask;
                result.SearchResultCount = result.Result.Count();
                //}
                //else
                //{
                //    result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, null);
                //}

            }
            catch (Exception ex)
            {
                result.ErrorMessage(DALConstants.DALErrorMessages.DAL_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> Add(params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (TEntity item in entities)
                {
                    if (item is IBaseEntity)
                    {
                        ((IBaseEntity)item).LastUpdatedUserId = activeUserId;
                    }
                    _context.Entry(item).State = EntityState.Added;
                }
                int value = _context.SaveChanges();
                if (value > 0) result.IsSucceeded = true;
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

        public ServiceResult<bool> AddWithTransaction(params TEntity[] entities)
        {
            foreach (var item in entities)
            {
                item.entityState = CoreEntityState.Added;
            }

            return ProcessEntityWithState(entities);
        }

        public ServiceResult<bool> Update(params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                ServiceResult<List<TEntity>> entitylist = GetByIdListTracking(entities.Select(t => t.Id).ToList());
                if (entitylist.IsSucceededAndDataIncluded())
                {
                    foreach (var item in entitylist.Result)
                    {
                        if (item is IBaseEntity)
                        {
                            ((IBaseEntity)item).LastUpdatedUserId = activeUserId;
                        }

                        var updatedEntity = _context.Entry(item);
                        updatedEntity.CurrentValues.SetValues(entities.FirstOrDefault(t => t.Id == item.Id));
                        updatedEntity.State = EntityState.Modified;
                    }
                }

                int value = _context.SaveChanges();
                if (value > 0) result.IsSucceeded = true;
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
            ServiceResult<List<TEntity>> entitylist = GetByIdListTracking(entities.Select(t => t.Id).ToList());
            if (entitylist.IsSucceededAndDataIncluded())
            {
                foreach (var item in entitylist.Result)
                {
                    if (item is IBaseEntity)
                    {
                        ((IBaseEntity)item).LastUpdatedUserId = activeUserId;
                    }

                    var updatedEntity = _context.Entry(item);
                    updatedEntity.CurrentValues.SetValues(entities.FirstOrDefault(t => t.Id == item.Id));
                    updatedEntity.State = EntityState.Modified;
                }
            }


            return ProcessEntityWithState(entitylist.Result.ToArray());
        }

        public ServiceResult<bool> Delete(params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (TEntity item in entities)
                {
                    if (item is IBaseEntity)
                    {
                        ((IBaseEntity)item).LastUpdatedUserId = activeUserId;
                    }
                    var updatedEntity = _context.Entry(item);
                    updatedEntity.State = EntityState.Deleted;
                }
                int value = _context.SaveChanges();
                if (value > 0) result.IsSucceeded = true;
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

        public ServiceResult<bool> DeleteWithTransaction(params Guid[] entityIds)
        {
            ServiceResult<List<TEntity>> entitylist = GetByIdListTracking(entityIds.ToList());
            if (entitylist.IsSucceededAndDataIncluded())
            {
                entitylist.Result.ForEach(t => t.entityState = CoreEntityState.Deleted);
            }

            return ProcessEntityWithState(entitylist.Result.ToArray());
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

        public ServiceResult<TEntity> GetById(Guid id)
        {
            ServiceResult<TEntity> result = new ServiceResult<TEntity>();

            try
            {
                result.Result = TableNoTracking.Where(t => t.Id.Equals(id)).FirstOrDefault();
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

        public ServiceResult<IList<TEntity>> FindList(Expression<Func<TEntity, bool>> filter, List<SortingExpression<TEntity>> sorters = null, int? page = null, int? pageSize = null)
        {
            ServiceResult<IList<TEntity>> result = new ServiceResult<IList<TEntity>>();

            try
            {
                IQueryable<TEntity> dbQuery = Entities;

                var query = dbQuery.AsQueryable();
                var countTask = query.Count();

                if (filter != null)
                    query = query.Where(filter);


                if (sorters != null && sorters.Count > 0)
                {
                    query = query.CustomSort(sorters);
                }

                if (page.HasValue && page.Value >= 0 && pageSize.HasValue)
                {
                    var skip = page.Value * pageSize.Value;

                    result.Result = query.Skip(skip).Take(pageSize.Value).AsNoTracking().ToList();
                }
                else
                {
                    result.Result = query.AsNoTracking().ToList();
                }

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

        public async Task<ServiceResult<IList<TEntity>>> FindListAsync(Expression<Func<TEntity, bool>> filter, List<SortingExpression<TEntity>> sorters = null, int? page = null, int? pageSize = null)
        {
            ServiceResult<IList<TEntity>> result = new ServiceResult<IList<TEntity>>();

            try
            {
                IQueryable<TEntity> dbQuery = Entities;

                var query = dbQuery.AsQueryable();
                var countTask = query.CountAsync();

                if (filter != null)
                    query = query.Where(filter);


                if (sorters != null && sorters.Count > 0)
                {
                    query = query.CustomSort(sorters);
                }

                Task<TEntity[]> resultsTask;

                if (page.HasValue && page.Value >= 0 && pageSize.HasValue)
                {
                    var skip = page.Value * pageSize.Value;

                    resultsTask = query.Skip(skip).Take(pageSize.Value).AsNoTracking().ToArrayAsync();
                }
                else
                {
                    resultsTask = query.AsNoTracking().ToArrayAsync();
                }

                await Task.WhenAll(resultsTask, countTask);

                if (countTask.IsCompletedSuccessfully && resultsTask.IsCompletedSuccessfully && result.Result != null)
                {
                    List<TEntity> resultList = resultsTask.Result.ToList();
                    if (sorters != null && sorters.Count > 0)
                    {
                        result.Result = resultList.CustomSort(sorters);
                    }
                    result.IsSucceeded = true;
                    result.Result = resultList;
                    result.TotalResultCount = countTask.Result;
                    result.SearchResultCount = result.Result.Count();
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

        public ServiceResult<bool> DeleteList(List<TEntity> entityList)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            IDbContextTransaction dbContextTransaction = null;
            try
            {
                using (dbContextTransaction = _context.Database.BeginTransaction())
                {
                    foreach (var item in entityList)
                    {
                        if (item is IBaseEntity)
                        {
                            ((IBaseEntity)item).LastUpdatedUserId = activeUserId;
                        }
                        var deletedEntity = _context.Entry(item);
                        deletedEntity.State = EntityState.Deleted;
                    }
                    int value = _context.SaveChanges();
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

        public virtual ServiceResult<bool> ProcessEntityWithState(params TEntity[] entities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();

            if (entities != null)
            {
                IDbContextTransaction dbContextTransaction = null;
                try
                {
                    using (dbContextTransaction = _context.Database.BeginTransaction())
                    {
                        foreach (var item in entities)
                        {
                            _context.Set<TEntity>().Add(item);
                            foreach (EntityEntry<IEntity> entry in _context.ChangeTracker.Entries<IEntity>())
                            {
                                IEntity entity = entry.Entity;
                                if (!entity.entityState.HasValue)
                                    entity.entityState = CoreEntityState.Unchanged;
                                entry.State = GetEntityState(entity.entityState.Value);
                            }
                        }

                        int value = _context.SaveChanges();
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

        public ServiceResult<bool> ProcessEntityWithStateNotTransaction(TEntity item)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                _entities.Add(item);
                foreach (EntityEntry<IEntity> entry in _context.ChangeTracker.Entries<IEntity>())
                {
                    IEntity entity = entry.Entity;
                    if (!entity.entityState.HasValue)
                        entity.entityState = CoreEntityState.Unchanged;
                    entry.State = GetEntityState(entity.entityState.Value);
                }

                int value = _context.SaveChanges();
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

        public ServiceResult<bool> ProcessEntitiesWithState(List<TEntity> items)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            IDbContextTransaction dbContextTransaction = null;
            try
            {
                using (dbContextTransaction = _context.Database.BeginTransaction())
                {
                    foreach (var item in items)
                    {
                        _entities.Add(item);
                        foreach (EntityEntry<IEntity> entry in _context.ChangeTracker.Entries<IEntity>())
                        {
                            IEntity entity = entry.Entity;
                            if (!entity.entityState.HasValue)
                                entity.entityState = CoreEntityState.Unchanged;
                            entry.State = GetEntityState(entity.entityState.Value);
                        }
                    }

                    int value = _context.SaveChanges();
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

        private void IsDeletedFilter(ref IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter = null)
        {
            //if (filter != null)
            //{
            //    if (typeof(TEntity).IsAssignableTo(typeof(IBaseEntity)))
            //    {
            //        filter = ExpressionBuilder.CombineWithAndAlso(filter, ExpressionBuilder.GetEqualsExpression<TEntity>(nameof(IBaseEntity.IsDeleted), false));
            //        //filter = Expression.Lambda<Func<TEntity, bool>>(expression, filter.Parameters);
            //    }
            //    query = query.Where(filter);
            //}
            //else
            //{
            //    if (typeof(TEntity).IsAssignableTo(typeof(IBaseEntity)))
            //    {
            //        query = query.Where(ExpressionBuilder.GetEqualsExpression<TEntity>(nameof(IBaseEntity.IsDeleted), false));
            //    }
            //}
        }
    }
}
