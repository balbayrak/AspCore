using AspCore.Authentication.Concrete;
using AspCore.Entities.Configuration;

namespace AspCore.Authentication.Abstract
{
    public interface IAppSettingsApiAuthenticationProvider<TInput, TOutput, TOption> : IApiAuthenticationProvider<TInput, TOutput>
       where TInput : AuthenticationInfo
       where TOutput : class, new()
       where TOption : class, IConfigurationEntity, new()
    {
        
    }
}
