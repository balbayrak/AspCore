using AspCore.Entities.General;

namespace AspCore.Business.Task.Abstract
{
    public interface ITaskValidator
    {
        ServiceResult<bool> Validate();
    }
}
