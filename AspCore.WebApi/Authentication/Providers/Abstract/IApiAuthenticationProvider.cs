using AspCore.Entities.Authentication;
using AspCore.Entities.General;
using System.Collections.Generic;

namespace AspCore.WebApi.Authentication.Providers.Abstract
{
    public interface IApiAuthenticationProvider<TInput, TOutput> 
       where TInput : AuthenticationInfo
       where TOutput : class, new()
    {
        ServiceResult<TOutput> Authenticate(TInput input);

        ServiceResult<bool> AuthorizeAction(string actionName, IDictionary<string, object> arguments = null);
    }
}
