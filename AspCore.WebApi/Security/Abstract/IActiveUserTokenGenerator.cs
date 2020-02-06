using AspCore.Business.Security.Abstract;
using AspCore.Entities.User;

namespace AspCore.WebApi.Security.Abstract
{
    public interface IActiveUserTokenGenerator : ITokenGenerator<ActiveUser>
    {
    }
}
