using AspCore.Dependency.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Dependency.Concrete
{
    public static class ServiceProviderExt
    {
        public static T GetServiceByName<T>(this IServiceProvider serviceProvider, string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                var factory = (IServiceByNameFactory<T>)serviceProvider.GetService(typeof(IServiceByNameFactory<T>));
                if (factory != null)
                {
                    return factory.GetByName(serviceProvider, name);
                }
            }
            return default(T);
        }
    }
}
