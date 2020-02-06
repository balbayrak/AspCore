using AspCore.Entities.Configuration;

namespace AspCore.WebApi.Configuration.Options
{
    public class AppSettingsAuthProviderOption<TOption>
            where TOption : class, IConfigurationEntity, new()
    {
        public string configurationKey { get; set; }
        public TOption option { get; set; }
    }
}
