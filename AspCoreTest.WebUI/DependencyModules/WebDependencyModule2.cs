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
    public class WebDependencyModule2 : AspCoreDependencyModule
    {
        public WebDependencyModule2(IServiceCollection services) : base(services)
        {
                
        }
        public override void ConfigureServices()
        {
            services.BindType<IPersonBff, PersonBff3>(option=>
            {
                option.serviceLifetime = ServiceLifetime.Transient;
            });
        }
    }
}
