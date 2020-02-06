using Microsoft.Extensions.DependencyInjection;

namespace AspCore.Dependency.Configuration
{
    public class DependencyOption
    {
        public DependencyOption()
        {
            this.namespaceStr = null;
            this.serviceLifetime = ServiceLifetime.Scoped;
        }
        public string namespaceStr { get; set; }

        public ServiceLifetime serviceLifetime { get; set; }
    }
}
