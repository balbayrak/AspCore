using System.Collections.Generic;
using AspCore.Authentication.Concrete;
using AspCore.Entities.General;

namespace AspCore.Authentication.Abstract
{
    public interface IApiAuthenticationProvider<TInput, TOutput> : IAuthenticationService
       where TInput : AuthenticationInfo
       where TOutput : class, new()
    {
        ServiceResult<TOutput> Authenticate(TInput input);

        ServiceResult<bool> AuthorizeAction(string actionName, IDictionary<string, object> arguments = null);
    }
}
