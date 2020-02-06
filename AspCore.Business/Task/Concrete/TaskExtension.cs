using System;
using System.Collections.Generic;
using AspCore.Business.General;
using AspCore.Business.Task.Abstract;
using AspCore.Entities.General;

namespace AspCore.Business.Task.Concrete
{
    public static class TaskExtension
    {
        public static void TaskErrorMessage<TResult>(this ServiceResult<TResult> result, Exception ex)
        {
            result.ErrorMessage = BusinessConstants.BaseExceptionMessages.TASK_EXCEPTION;
            result.ExceptionMessage = ex.Message + "---> stacktrace:" + ex.StackTrace;
        }

        public static void Add(this List<ValidationItem> list, ITaskValidator taskValidator, params string[] operations)
        {
            list.Add(new ValidationItem(taskValidator, operations));
        }
    }
}
