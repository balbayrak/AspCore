using Microsoft.Extensions.DependencyInjection;

namespace AspCore.Entities.Configuration
{
    public class ConfigurationOption
    {
        protected IServiceCollection _services { get; private set; }

        public ConfigurationOption(IServiceCollection services)
        {
            _services = services;
        }
    }
}
