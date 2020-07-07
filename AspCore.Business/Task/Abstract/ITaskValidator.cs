using AspCore.Entities.General;
using System.Threading.Tasks;

namespace AspCore.Business.Task.Abstract
{
    public interface ITaskValidator
    {
        Task<ServiceResult<bool>> Validate();
    }
}
