using AspCore.Dependency.Abstract;
using AspCore.Entities.General;
using System.Threading.Tasks;

namespace AspCore.Business.Task.Abstract
{
    public interface ITaskFlowBuilder : ITransientType
    {
        void AddTask(ITask task);

        Task<BaseServiceResult> RunTasks();

        Task<BaseServiceResult> ValidateAllTasks();
    }
}
