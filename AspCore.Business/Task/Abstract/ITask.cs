using AspCore.Entities.General;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspCore.Business.Task.Abstract
{
    public interface ITask : IDisposable
    {
        List<ITaskValidator> Validators { get; }
        ITask AddValidator<T>(T validator) where T : class, ITaskValidator;
        Task<BaseServiceResult> Run();
    }
}
