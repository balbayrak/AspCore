//using AspCore.Business.Task.Concrete;
//using AspCore.DataAccess.Abstract;
//using AspCore.Dependency.Concrete;
//using AspCore.Entities.EntityType;
//using AspCore.Entities.General;

//namespace AspCore.Business.Task.Abstract
//{
//    public abstract class CacheEntityTask<TEntity, TDAL> : EntityTask<TEntity, TDAL>
//        where TEntity : class, IEntity, new()
//        where TDAL : IEntityRepository<TEntity>
//    {
//        private readonly ICacheHelper<TEntity> cacheHelper;
//        public CacheEntityTask(TaskEntity<TEntity> taskEntity, string operation) : base(taskEntity)
//        {
//            cacheHelper = DependencyResolver.Current.GetService<ICacheHelper<TEntity>>();
//        }

//        internal override ServiceResult<TResult> Create<TResult>()
//        {
//            _transactionBuilder.BeginTransaction();

//            ServiceResult<TResult> result = new ServiceResult<TResult>();
//            try
//            {
//                ServiceResult<bool> resultDAL = _dataLayer.Add(taskEntity.entity);
//                if (resultDAL.IsSucceeded)
//                {
//                    ServiceResult<bool> resultCache = cacheHelper.Create(taskEntity.entity);
//                    if (resultCache.IsSucceeded)
//                    {
//                        _transactionBuilder.CommitTransaction();
//                        result.IsSucceeded = true;
//                    }
//                }
//            }
//            catch
//            {
//                _transactionBuilder.RollbackTransaction();
//            }
//            finally
//            {
//                _transactionBuilder.DisposeTransaction();
//            }

//            return result;
//        }

//        internal override ServiceResult<TResult> Update<TResult>()
//        {
//            _transactionBuilder.BeginTransaction();

//            ServiceResult<TResult> result = new ServiceResult<TResult>();
//            try
//            {
//                ServiceResult<bool> resultDAL = _dataLayer.Update(taskEntity.entity);
//                if (resultDAL.IsSucceeded)
//                {
//                    ServiceResult<bool> resultCache = cacheHelper.Update(taskEntity.entity);
//                    if (resultCache.IsSucceeded)
//                    {
//                        _transactionBuilder.CommitTransaction();
//                        result.IsSucceeded = true;
//                    }
//                }
//            }
//            catch
//            {
//                _transactionBuilder.RollbackTransaction();
//            }
//            finally
//            {
//                _transactionBuilder.DisposeTransaction();
//            }

//            return result;
//        }

//        internal override ServiceResult<TResult> Delete<TResult>()
//        {
//            _transactionBuilder.BeginTransaction();

//            ServiceResult<TResult> result = new ServiceResult<TResult>();
//            try
//            {
//                ServiceResult<bool> resultDAL = _dataLayer.Delete(taskEntity.entity);
//                if (resultDAL.IsSucceeded)
//                {
//                    ServiceResult<bool> resultCache = cacheHelper.Delete(taskEntity.entity);
//                    if (resultCache.IsSucceeded)
//                    {
//                        _transactionBuilder.CommitTransaction();
//                        result.IsSucceeded = true;
//                    }
//                }
//            }
//            catch
//            {
//                _transactionBuilder.RollbackTransaction();
//            }
//            finally
//            {
//                _transactionBuilder.DisposeTransaction();
//            }

//            return result;
//        }
//    }
//}
