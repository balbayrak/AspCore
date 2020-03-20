using AspCore.ConfigurationAccess.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Configuration;
using AspCore.Entities.General;
using AspCore.WebApi.Authentication.JWT.Concrete;
using AspCore.WebApi.Authentication.Providers.Abstract;
using System;
using System.Collections.Generic;

namespace AspCore.WebApi.Authentication.Providers.Concrete
{
    public abstract class AppSettingsAuthenticationProvider<TInput, TOutput, TOption> : IAppSettingsApiAuthenticationProvider<TInput, TOutput, TOption>
    where TInput : AuthenticationInfo
   where TOutput : class, new()
   where TOption : class, IConfigurationEntity, new()
    {
        protected TOption _option { get; private set; }
        protected IConfigurationAccessor configurationHelper { get; private set; }
        public AppSettingsAuthenticationProvider(string configurationKey, TOption option = null)
        {
            configurationHelper = DependencyResolver.Current.GetService<IConfigurationAccessor>();
            _option = configurationHelper.GetValueByKey<TOption>(configurationKey);

            if (option == null && !string.IsNullOrEmpty(configurationKey))
            {
                _option = configurationHelper.GetValueByKey<TOption>(configurationKey);
                if (_option == null)
                {
                    throw new Exception(SecurityConstants.TOKEN_SETTING_OPTIONS.OPTION_KEY_IS_NULL_EXCEPTION);
                }
            }
            else if (option != null && string.IsNullOrEmpty(configurationKey))
            {
                _option = option;
            }
        }

        public abstract ServiceResult<TOutput> AuthenticateClient(TInput input);
        public abstract ServiceResult<bool> AuthorizeActionInternal(string actionName, IDictionary<string, object> arguments = null);
        public ServiceResult<TOutput> Authenticate(TInput input)
        {
            return AuthenticateClient(input);
        }
        public ServiceResult<bool> AuthorizeAction(string actionName, IDictionary<string, object> arguments = null)
        {
            return AuthorizeActionInternal(actionName, arguments);
        }
    }
}
