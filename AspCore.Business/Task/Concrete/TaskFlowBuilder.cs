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
        public TaskFlowBuilder(ITransactionBuilder transactionBuilder)
        {
            _taskList = new List<TaskFlowItem>();
            _transactionBuilder = transactionBuilder;
        }

        public void AddTask(ITask task)
        {
            _taskList.Add(new TaskFlowItem(task, _taskList.Count));
        }

        public ServiceResult<TResult> RunTasks<TResult>()
        {
            ServiceResult<TResult> result = new ServiceResult<TResult>();
            _transactionBuilder.BeginTransaction();
            try
            {
               ServiceResult<bool> validationResult = ValidateAllTasks();
                if (validationResult.IsSucceeded)
                {

                    foreach (var taskItem in _taskList)
                    {
                        result = taskItem._task.Run<TResult>();
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
                else
                {
                    result.ErrorMessage = validationResult.ErrorMessage;
                    result.ExceptionMessage = validationResult.ExceptionMessage;
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
