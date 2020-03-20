using AspCore.Entities.User;
using AspCore.WebApi.Authentication.Abstract;

namespace AspCore.WebApi.Security.Abstract
{
    public interface IActiveUserTokenGenerator : ITokenGenerator<ActiveUser>
    {
    }
}
