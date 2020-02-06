using AspCore.Dependency.Abstract;
using AspCore.Entities.User;

namespace AspCore.Web.Abstract
{
    public interface ICurrentUser : ISingletonType
    {
        ActiveUser currentUser { get; }
    }
}
