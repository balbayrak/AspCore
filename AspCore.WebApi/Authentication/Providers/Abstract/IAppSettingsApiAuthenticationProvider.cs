using AspCore.Entities.Authentication;
using AspCore.Entities.Configuration;

namespace AspCore.WebApi.Authentication.Providers.Abstract
{
    public interface IAppSettingsApiAuthenticationProvider<TInput, TOutput, TOption> : IApiAuthenticationProvider<TInput, TOutput>
       where TInput : AuthenticationInfo
       where TOutput : class, new()
       where TOption : class, IConfigurationEntity, new()
    {
        
    }
}
