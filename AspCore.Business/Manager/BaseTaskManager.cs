using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.Business.Task.Abstract;
using AspCore.Business.Task.Concrete;
using AspCore.Dependency.Concrete;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Extension;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace AspCore.Business.Manager
{
    public abstract class BaseTaskManager<TActiveUser, TEntity, TTaskBuilder> : ITaskService<TActiveUser, TEntity>
        where TEntity : class, new()
        where TActiveUser : class, IActiveUser, new()
        where TTaskBuilder : TaskBuilder, ITaskBuilder
    {
        protected ITaskBuilder _taskBuilder;

        protected ITaskFlowBuilder _taskFlowBuilder;

        private IHttpContextAccessor _httpContextAccessor;
        public TActiveUser activeUser { get; private set; }

        public BaseTaskManager()
        {
            _httpContextAccessor = DependencyResolver.Current.GetService<IHttpContextAccessor>();
            _taskBuilder = DependencyResolver.Current.GetService<TTaskBuilder>();
            _taskFlowBuilder = DependencyResolver.Current.GetService<ITaskFlowBuilder>();

            ServiceResult<TActiveUser> activeUserResult = _httpContextAccessor.HttpContext.GetActiveUserInfo<TActiveUser>();
            if (activeUserResult.IsSucceededAndDataIncluded())
            {
                activeUser = activeUserResult.Result;
            }
            else
            {
                throw new Exception(BusinessConstants.BaseExceptionMessages.TASK_USER_INFO_NOT_FOUND);
            }
        }

        public virtual ServiceResult<bool> RunAction(params TaskEntity<TEntity>[] taskEntities)
        {
            if (taskEntities.Length == 1)
                return TaskRun(taskEntities[0]);
            else
            {
                return TaskListRun(taskEntities);
            }
        }

        public virtual ServiceResult<bool> RunAction(List<TaskEntity<TEntity>> taskEntities)
        {
            if (taskEntities.Count == 1)
                return TaskRun(taskEntities[0]);
            else
            {
                return TaskListRun(taskEntities);
            }
        }

        private ServiceResult<bool> TaskRun(TaskEntity<TEntity> taskEntity)
        {
            ITask task = _taskBuilder.GenerateTask<TEntity, bool>(taskEntity);
            return task.Run<bool>();
        }

        private ServiceResult<bool> TaskListRun(TaskEntity<TEntity>[] taskEntityList)
        {
            foreach (var item in taskEntityList)
            {
                ITask task = _taskBuilder.GenerateTask<TEntity, bool>(item);
                _taskFlowBuilder.AddTask(task);
            }

            return _taskFlowBuilder.RunTasks();
        }

        private ServiceResult<bool> TaskListRun(List<TaskEntity<TEntity>> taskEntityList)
        {
            foreach (var item in taskEntityList)
            {
                ITask task = _taskBuilder.GenerateTask<TEntity, bool>(item);
                _taskFlowBuilder.AddTask(task);
            }

            return _taskFlowBuilder.RunTasks();
        }

    }
}
