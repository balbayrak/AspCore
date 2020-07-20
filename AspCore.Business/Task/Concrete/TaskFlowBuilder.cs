using AspCore.Business.Task.Abstract;
using AspCore.DataAccess.Abstract;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<BaseServiceResult> RunTasks()
        {
            BaseServiceResult result = new BaseServiceResult();
            _transactionBuilder.BeginTransaction();
            try
            {
                BaseServiceResult validationResult = await ValidateAllTasks();
                if (validationResult.IsSucceeded)
                {

                    foreach (var taskItem in _taskList)
                    {
                        result = await taskItem.task.Run();
                        if (result.IsSucceeded)
                        {
                            continue;
                        }
                        _transactionBuilder.RollbackTransaction();
                        break;
                       
                    }

                    if (result.IsSucceeded)
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

        public async Task<BaseServiceResult> ValidateAllTasks()
        {
            BaseServiceResult validateResult = new BaseServiceResult();
            try
            {
                foreach (var taskItem in _taskList)
                {
                    foreach (var validator in taskItem.task.Validators)
                    {
                        validateResult = await validator.Validate();
                        if (!validateResult.IsSucceeded) break;
                        else continue;
                    }

                    if (!validateResult.IsSucceeded) break;
                    else continue;
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
