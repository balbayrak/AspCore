using AspCore.Dependency.Abstract;
using AspCore.Entities.General;

namespace AspCore.Business.Task.Abstract
{
    public interface ITaskFlowBuilder : ITransientType
    {
        void AddTask(ITask task);

        ServiceResult<bool> RunTasks();

        ServiceResult<bool> ValidateAllTasks();
    }
}
