using Microsoft.Extensions.DependencyInjection;

namespace AspCore.Entities.Configuration
{
    public class ConfigurationOption
    {
        public IServiceCollection services { get; private set; }

        public ConfigurationOption(IServiceCollection services)
        {
            this.services = services;
        }
    }
}
