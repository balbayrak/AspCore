using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using AspCore.Dependency.Abstract;

namespace AspCore.Dependency.Concrete
{
    public class DependencyResolver
    {
        private static DependencyResolver _resolver;

        public static DependencyResolver Current
        {
            get
            {
                if (_resolver == null)
                    throw new Exception("DependencyResolver not initialized. You should initialize it in Startup class");
                return _resolver;
            }
        }

        public static void Init(IServiceProvider services)
        {
            if (_resolver == null)
                _resolver = new DependencyResolver(services);
        }

        private readonly IServiceProvider _serviceProvider;

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public T GetServiceByName<T>(string name)
        {
            var factory = _serviceProvider.GetService<IServiceByNameFactory<T>>();
            if (factory != null)
            {
                return factory.GetByName(_serviceProvider, name);
            }

            return default(T);
        }

        private DependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
