using System;
using System.Collections.Generic;
using AspCore.Business.Task.Abstract;
using AspCore.DataAccess.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.General;

namespace AspCore.Business.Task.Concrete
{
    public class TaskFlowBuilder : ITaskFlowBuilder
    {
        private List<TaskFlowItem> _taskList { get; set; }
        private ITransactionBuilder _transactionBuilder;
        public TaskFlowBuilder()
        {
            _taskList = new List<TaskFlowItem>();
        }

        public void AddTask(ITask task)
        {
            _taskList.Add(new TaskFlowItem(task, _taskList.Count));
            _transactionBuilder = DependencyResolver.Current.GetService<ITransactionBuilder>();
        }

        public ServiceResult<bool> RunTasks()
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            _transactionBuilder.BeginTransaction();
            try
            {
                result = ValidateAllTasks();
                if (result.IsSucceeded)
                {

                    foreach (var taskItem in _taskList)
                    {
                        result = taskItem._task.Run<bool>();
                        if (!result.IsSucceeded)
                        {
                            _transactionBuilder.RollbackTransaction();
                            break;
                        }
                        else continue;
                    }

                    if(result.IsSucceeded)
                    {
                        _transactionBuilder.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                _transactionBuilder.RollbackTransaction();
                result.IsSucceeded = false;
                result.TaskErrorMessage(ex);
            }
            finally
            {
                _transactionBuilder.DisposeTransaction();
            }

            return result;
        }

        public ServiceResult<bool> ValidateAllTasks()
        {
            ServiceResult<bool> validateResult = new ServiceResult<bool>();
            try
            {
                foreach (var taskItem in _taskList)
                {
                    if (!taskItem._task.SkipValidate)
                    {
                        validateResult = taskItem._task.Validate();
                        if (!validateResult.IsSucceeded) break;
                        else continue;
                    }
                }
            }
            catch (Exception ex)
            {
                validateResult.IsSucceeded = false;
                validateResult.TaskErrorMessage(ex);
            }

            return validateResult;
        }
    }
}
