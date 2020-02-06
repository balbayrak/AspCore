using System;
using System.Collections.Generic;
using System.Reflection;
using AspCore.Business.General;
using AspCore.Business.Task.Concrete;
using AspCore.Entities.General;

namespace AspCore.Business.Task.Abstract
{
    public abstract class CoreTask : ITask
    {
        public virtual bool SkipValidate
        {
            get
            {
                return false;
            }
        }
        protected CoreTaskEntity coreTaskEntity { get; private set; }
        public CoreTask(CoreTaskEntity taskEntity)
        {
            this.coreTaskEntity = taskEntity;
        }
        internal ServiceResult<TResult> RunTask<TResult>()
        {
            ServiceResult<TResult> result = new ServiceResult<TResult>();

            MethodInfo methodInfo = this.GetType().GetMethod(coreTaskEntity.actionName);

            if (methodInfo != null)
            {
                result = methodInfo.Invoke(this, null) as ServiceResult<TResult>;
            }
            else
            {
                result.ErrorMessage = BusinessConstants.BaseExceptionMessages.TASK_ACTION_EXCEPTION;
            }

            if (result == null)
            {
                result = new ServiceResult<TResult>();
                result.ErrorMessage = BusinessConstants.BaseExceptionMessages.TASK_ACTION_RUN_EXCEPTION;
            }
            return result;
        }

        internal abstract List<ValidationItem> GetValidators();
        public ServiceResult<bool> Validate()
        {
            ServiceResult<bool> validationResult = new ServiceResult<bool>();
            validationResult.IsSucceeded = true;
            validationResult.WarningMessage = null;
            validationResult.ErrorMessage = null;
            validationResult.ExceptionMessage = null;

            List<ValidationItem> validators = GetValidators();

            if (validators != null && validators.Count > 0)
            {
                foreach (var item in validators)
                {
                    if (item._operations == null || (item._operations != null && item._operations.Count == 0) || item._operations.Contains(coreTaskEntity.actionName))
                        validationResult = item._taskValidator.Validate();
                    if (!validationResult.IsSucceeded) break;
                }
            }


            return validationResult;
        }
        public ServiceResult<TResult> Run<TResult>()
        {
            ServiceResult<TResult> tResult = null;
            try
            {
                ServiceResult<bool> validateResult = new ServiceResult<bool> { IsSucceeded = true };
                if (!SkipValidate)
                {
                    validateResult = Validate();
                }
                if (validateResult.IsSucceeded)
                {
                    tResult = RunTask<TResult>();
                    if (!tResult.IsSucceeded)
                    {
                        try
                        {
                            RollBack();
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    tResult = new ServiceResult<TResult>();
                    tResult.IsSucceeded = false;
                    tResult.WarningMessage = validateResult.WarningMessage;
                    tResult.ErrorMessage = validateResult.ErrorMessage;
                    tResult.ExceptionMessage = validateResult.ExceptionMessage;
                }
            }
            catch (Exception ex)
            {
                tResult = new ServiceResult<TResult>();
                tResult.IsSucceeded = false;
                tResult.WarningMessage = null;
                tResult.TaskErrorMessage(ex);

                try
                {
                    RollBack();
                }
                catch
                {

                }
            }

            return tResult;
        }
        public virtual ServiceResult<bool> RollBack()
        {
            return new ServiceResult<bool> { IsSucceeded = true };
        }
        public void Dispose()
        {
            coreTaskEntity = null;
        }
    }
}
