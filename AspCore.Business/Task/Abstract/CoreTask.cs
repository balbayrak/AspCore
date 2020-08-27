using AspCore.Business.Task.Concrete;
using AspCore.Entities.General;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspCore.Business.Task.Abstract
{
    public abstract class CoreTask<TInput, TOutput> : ITask
    {
        protected TInput Input { get; private set; }
        public CoreTask(TInput input)
        {
            Input = input;
        }

        public abstract Task<ServiceResult<TOutput>> RunTask();

        private List<ITaskValidator> _Validators { get; set; }

        public List<ITaskValidator> Validators => this._Validators;

        public async Task<ServiceResult<bool>> Validate()
        {
            ServiceResult<bool> validationResult = new ServiceResult<bool>();
            validationResult.IsSucceeded = true;
            validationResult.Result = true;
            validationResult.WarningMessage = null;
            validationResult.ErrorMessage = null;
            validationResult.ExceptionMessage = null;

            if (Validators != null && Validators.Count > 0)
            {
                foreach (var validator in Validators)
                {
                    validationResult = await validator.Validate();
                    if (!validationResult.IsSucceeded) break;
                }
            }

            return validationResult;
        }

        public async Task<BaseServiceResult> Run()
        {
            ServiceResult<TOutput> tResult;
            try
            {
                ServiceResult<bool> validationResult = await Validate();

                if (validationResult.IsSucceeded && validationResult.Result)
                {
                    tResult = await RunTask();
                    if (!tResult.IsSucceeded)
                    {
                        ServiceResult<bool> rollbackResult = await RollBack();
                        if (!rollbackResult.IsSucceeded)
                        {
                            tResult.WarningMessage = $"{ tResult.WarningMessage ?? string.Empty}_rollbackResult: {rollbackResult.WarningMessage ?? string.Empty}";
                            tResult.ErrorMessage = $"{ tResult.ErrorMessage ?? string.Empty}_rollbackResult: {rollbackResult.ErrorMessage ?? string.Empty}";
                            tResult.ExceptionMessage = $"{ tResult.ExceptionMessage ?? string.Empty}_rollbackResult: {rollbackResult.ExceptionMessage ?? string.Empty}";
                        }
                    }
                }
                else
                {
                    tResult = new ServiceResult<TOutput>();
                    tResult.IsSucceeded = false;
                    tResult.WarningMessage = validationResult.WarningMessage;
                    tResult.ErrorMessage = validationResult.ErrorMessage;
                    tResult.ExceptionMessage = validationResult.ExceptionMessage;
                }
            }
            catch (Exception ex)
            {
                tResult = new ServiceResult<TOutput>();
                tResult.IsSucceeded = false;
                tResult.WarningMessage = null;
                tResult.TaskErrorMessage(ex);

                ServiceResult<bool> rollbackResult = await RollBack();
                if (!rollbackResult.IsSucceeded)
                {
                    tResult.WarningMessage = $"{ tResult.WarningMessage ?? string.Empty}_rollbackResult: {rollbackResult.WarningMessage ?? string.Empty}";
                    tResult.ErrorMessage = $"{ tResult.ErrorMessage ?? string.Empty}_rollbackResult: {rollbackResult.ErrorMessage ?? string.Empty}";
                    tResult.ExceptionMessage = $"{ tResult.ExceptionMessage ?? string.Empty}_rollbackResult: {rollbackResult.ExceptionMessage ?? string.Empty}";
                }
            }

            return tResult;
        }

        public virtual Task<ServiceResult<bool>> RollBack()
        {
            return System.Threading.Tasks.Task.FromResult(new ServiceResult<bool> { IsSucceeded = true });
        }

        public ITask AddValidator<T>(T validator) where T : class, ITaskValidator
        {
            this._Validators = this._Validators ?? new List<ITaskValidator>();
            this._Validators.Add(validator);

            return this;
        }

        public void Dispose()
        {

        }
    }
}
