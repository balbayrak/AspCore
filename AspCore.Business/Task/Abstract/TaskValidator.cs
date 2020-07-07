using AspCore.Entities.General;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.Business.Task.Abstract
{
    public abstract class TaskValidator<TInput>
    {
        protected TInput Input { get; private set; }

        public TaskValidator(TInput input)
        {
            Input = input;
        }
        public abstract Task<ServiceResult<bool>> Validate();
    }
}
