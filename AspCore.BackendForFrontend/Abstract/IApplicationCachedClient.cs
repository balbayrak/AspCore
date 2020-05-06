using AspCore.Dependency.Abstract;
using AspCore.Entities.User;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IApplicationCachedClient : ISingletonType
    {
        string ApplicationUserKey { get; }
        T GetAuthenticatedUser<T>() where T : class, IAuthenticatedUser, new();
    }
}
