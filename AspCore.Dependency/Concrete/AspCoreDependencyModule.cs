using AspCore.Dependency.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Dependency.Concrete
{
    public abstract class AspCoreDependencyModule : IDependencyModule
    {
        protected IServiceCollection services { get; private set; }
        public AspCoreDependencyModule(IServiceCollection services)
        {
            this.services = services;
        }

        public abstract void ConfigureServices();
    }
}
