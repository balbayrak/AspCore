using System;
using AspCore.Entities.General;

namespace AspCore.Business.Task.Abstract
{
    public interface ITask : IDisposable
    {
        bool SkipValidate { get; }
        ServiceResult<bool> Validate();
        ServiceResult<TResult> Run<TResult>();
        ServiceResult<bool> RollBack();
    }
}
