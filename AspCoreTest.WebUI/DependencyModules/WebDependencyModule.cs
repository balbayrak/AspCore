using AspCore.Dependency.Concrete;
using AspCore.Dependency.Configuration;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Bffs.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreTest.WebUI.DependencyModules
{
    public class WebDependencyModule : AspCoreDependencyModule
    {
        public WebDependencyModule(IServiceCollection services) : base(services)
        {
                
        }
        public override void ConfigureServices()
        {
            services.BindType<IPersonBff, PersonBff2>(option=>
            {
                option.serviceLifetime = ServiceLifetime.Transient;
            });
        }
    }
}
